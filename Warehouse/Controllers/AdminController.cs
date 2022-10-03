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
using Warehouse.Core.RequestParameters;

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
        public AdminController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, WarehouseDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            List<AppUser> users = await _userManager.Users.ToListAsync();
            List<UserShowDto> userShowDto = _mapper.Map<List<UserShowDto>>(users);

            if (users == null)
            {
                return NotFound();
            }

            return Ok(userShowDto);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
            UserShowDto userShowDto = _mapper.Map<UserShowDto>(appUser);

            return Ok(userShowDto);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateDto userCreateDto)
        {
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



        [HttpPut]
        public async Task<IActionResult> DeactiveOrActiveUser(string id, bool status)
        {

            try
            {
                var user = await _userManager.Users.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
                user.Status = status;
                await _userManager.UpdateAsync(user);
            }
            catch (Exception)
            {

                return BadRequest();
            }
            string message = status == true ? "User Aktiv olundu" : "User Deaktiv olundu";
            return Ok(new { success = message });
        }




        [HttpGet]
        public  IActionResult getInfo([FromQuery]Pagination pagination)
        {
            var totalProduct = _context.Products.Count();
            var products = (from x in _context.Products
                           select new
                           {
                               x.Id,
                               x.Name,
                               x.Description,

                           }).Skip(pagination.Size * pagination.Page).Take(pagination.Size);
            return Ok(new { totalProduct, products});
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