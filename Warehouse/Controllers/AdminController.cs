using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Warehouse.Data.Dto.AppUsers;
using Warehouse.Data.Models;
using Warehouse.Data.Models.Common.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Warehouse.Core.Configuration;
using Warehouse.Core.Services.EmailService;
using Warehouse.Core.Helpers.Wrappers;

namespace Warehouse.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly WarehouseDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public AdminController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, WarehouseDbContext context, IMapper mapper, IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] PaginationFilter pFilter, string? search, string sortOrder)
        {

            IQueryable<AppUser> users = _userManager.Users.IgnoreQueryFilters();

            if (users == null)
            {
                return NotFound(new Response<object>() { Message = "Istifadəçi tapılmadı" });
            }


            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(x => x.UserName.Contains(search) || x.Email.Contains(search));
            }
            switch (sortOrder)
            {
                case "nameDesc":
                    users = users.OrderByDescending(x => new { x.UserName, x.Email });
                    break;
                default:
                    break;
            }

            var returnedUsers = users.Skip((pFilter.PageNumber - 1) * pFilter.PageSize)
                .Take(pFilter.PageSize).ToList();

            var data = _mapper.Map<IEnumerable<UserShowDto>>(returnedUsers);


            int totalRecord = users.Count();

            return Ok(new PagedResponse<IEnumerable<UserShowDto>>(data, pFilter.PageNumber, pFilter.PageSize, totalRecord));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null)
            {
                return NotFound(new Response<object>() { Message = "Istifadəçi tapılmadı" });
            }
            UserShowDto userShowDto = _mapper.Map<UserShowDto>(appUser);

            return Ok(new Response<UserShowDto>(userShowDto));
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateDto userCreateDto)
        {
            bool availableWarehousetoUser = _userManager.Users.Any(x => x.AnbarId == userCreateDto.AnbarId && x.Status == true);
            if (availableWarehousetoUser) return BadRequest(new Response<object> { Message = "Anbarin artiq bir aktiv isdifadecisi var " });
            if (ModelState.IsValid)
            {
                bool anbarIdValid = await _context.Anbars.AnyAsync(x => x.Id == userCreateDto.AnbarId);
                if (!anbarIdValid) return BadRequest(new { error = "Olmayan bir anbar girdiniz" });
                AppUser appUser = _mapper.Map<AppUser>(userCreateDto);
                IdentityResult result = await _userManager.CreateAsync(appUser, userCreateDto.PasswordHash.Trim());



                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("User").Result)
                    {
                        AppRole appRole = new AppRole()
                        {
                            Name = "User"
                        };
                        IdentityResult roleResult = await _roleManager.CreateAsync(appRole);
                        if (roleResult.Succeeded)
                        {
                            _userManager.AddToRoleAsync(appUser, "User").Wait();
                        }

                    }
                    _userManager.AddToRoleAsync(appUser, "User").Wait();
                    return CreatedAtAction("CreateUser", userCreateDto);
                }
                else
                {
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                }


            }
            return BadRequest(ModelState);
        }

        [HttpPut()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeUserPassword(ChangeUserPasswordDto changeUserPasswordDto)
        {
            //AppUser currentUser = HttpContext.User.Identity as AppUser;
            AppUser user = await _userManager.FindByEmailAsync(changeUserPasswordDto.Email);
            //var a = _userManager
            if (user == null) return BadRequest(new Response<object>() { Message = "User tapilmadi" });
            if (changeUserPasswordDto.newPassword != changeUserPasswordDto.PasswordConfirmation) return BadRequest(new Response<object>() { Message = "Password eyni olmalidir" });
            await _userManager.RemovePasswordAsync(user);

            var result = await _userManager.AddPasswordAsync(user, changeUserPasswordDto.newPassword);
            if (result.Succeeded)
            {
                return Ok(new Response<object>() { Succeeded = true, Message = "Parol guncellendi" });
            }
            return BadRequest(new Response<object>() { Succeeded = false, Message = "Parol yenilenerken bir xeta yarandi" });
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> DeactiveOrActiveUser(string id, bool status)
        {

            var user = await _userManager.Users.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return NotFound(new Response<object> { Message = "Bele bir user tapilmadi" });
            user.Status = status;
            await _userManager.UpdateAsync(user);
            string message = status == true ? "User Aktiv olundu" : "User Deaktiv olundu";
            if (status)
            {
                return Ok(new Response<object>() { Message = message });
            }
            return Ok(new Response<object>() { Message = message });


        }


        [HttpPut("{transactionno}")]
        public async Task<IActionResult> AcceptUserTransaction(string transactionno, bool accept)
        {
            Transaction transaction = _context.Transactions.Where(x => x.TransactionNo == transactionno).FirstOrDefault();
            if (transaction == null) return NotFound(new Response<object> { Message = $"{transactionno} nomreli transaction tapilmadi" });
            if (accept)
            {
                transaction.Status = true;
                _context.Entry(transaction).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest(new Response<object> { Message = "Transactionu yanliz qebul ede bilersiniz.." });
            }



            return Ok(new Response<object> { Succeeded = true, Message = "Mehsulu qebul etdiniz.." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserTransactionsbyId(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);

            var userTransaction = await (from tr in _context.Transactions
                                         where tr.receiver_id == user.AnbarId || tr.sender_id == user.AnbarId
                                         orderby tr.CreatedDate
                                         select new
                                         {
                                             tr.Id,
                                             TransactionNo = tr.TransactionNo,
                                             Hardan = tr.Sender.Name == null ? "Import" : tr.Sender.Name,
                                             Hara = tr.Receiver.Name,
                                             Mehsul = tr.Product.Name,
                                             Kateqoriyasi = tr.Product.Category.Name,
                                             Miqdar = tr.Count,
                                             Veziyyeti = tr.Status == false ? "Gozlemede" : "Qebul edildi",
                                             Nevaxt = tr.CreatedDate.ToString("yyyy-MM-dd : HH-mm-ss"),
                                             Kim = tr.User.UserName

                                         }).ToListAsync();

            if (userTransaction == null) return NotFound(new Response<object>() { Message = "Transaction tapılmadı" });
            return Ok(new Response<object>(userTransaction));
        }


        [HttpGet]
        public IActionResult getInfo()
        {
            Message message = new Message(new List<string>() { "buludlumoka@gmail.com" }, "Test Email", "hello", null);
            _emailSender.SendEmailAsync(message);

            return Ok();
        }
    }
}


//[HttpPut, Route("UpdateUser{id}")]
//public async Task<IActionResult> UpdateUser(string id, [FromForm] UserUpdateDto model)
//{
//    AppUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
//    var result = await _userManager.RemovePasswordAsync(user);
//    if (result.Succeeded)
//    {
//        result = await _userManager.AddPasswordAsync(user, model.PasswordHash);
//        if (result.Succeeded)
//        {
//            return CreatedAtAction("UpdateUser", new { success = "user guncellendi" });
//        }
//        else
//        {
//            ModelState.AddModelError("", result.Errors.FirstOrDefault());
//        }
//    }
//    user.Status = model.Status;
//    user.PasswordHash = model.PasswordHash;
//    await _context.SaveChangesAsync();
//}