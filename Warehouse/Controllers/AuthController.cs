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
using Warehouse.Core.Services.EmailService;
using Warehouse.Core.Helpers.Wrappers;

namespace Warehouse.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly WarehouseDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration configuration;

        public AuthController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, WarehouseDbContext context, IMapper mapper, IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _mapper = mapper;
            _emailSender = emailSender;
            this.configuration = configuration;
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {

            //var a = configuration.GetSection("Url");
            //return Ok();
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(model.Email);
                //User deaktiv olunub
                //if (user.Status == false) { ModelState.AddModelError("ErrorMessage", "User Deaktiv olunub"); return BadRequest(ModelState); }
                if (user != null && user.Status != false && await _userManager.CheckPasswordAsync(user, model.Password.Trim()))
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    //var claims = new List<Claim>
                    //{
                    //    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    //};
                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey"));

                    var token = new JwtSecurityToken(
                        issuer: "http://karfree-001-site1.atempurl.com",
                        audience: "http://karfree-001-site1.atempurl.com",
                        expires: DateTime.UtcNow.AddMonths(1),
                        claims: claims,
                        signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                        );
                    var userrol=  await _userManager.GetRolesAsync(user);

                    return Ok(new
                    {
                        Role = userrol[0],
                        WarehouseId = user.AnbarId,
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    }); 
                }
                else
                {
                    ModelState.AddModelError("Error", "Email ve ya parol yanlisdir");
                }
            }
            return BadRequest(ModelState);
        }
     

        [HttpPut]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> ChangePassword(string OldPassword , string NewPassword)
        {
            //AppUser currentUser = HttpContext.User.Identity as AppUser;
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            //var a = _userManager

            var checkPass = await _userManager.CheckPasswordAsync(currentUser, OldPassword);
            if (!checkPass) return BadRequest(new Response<object> { Message = "Girdiyiniz parol yanlisdir" });

            var token = await _userManager.GeneratePasswordResetTokenAsync(currentUser);


            var result = await _userManager.ResetPasswordAsync(currentUser, token, NewPassword);
            if (result.Succeeded)
            {
                return Ok(new Response<object>() { Message= "Parol guncellendi" });
            }
            return BadRequest(new Response<object>() { Succeeded= false, Message= "Parol yenilenerken bir xeta yarandi"});
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> ForgetPassword( string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest(new { ErrorMessage = "Bele bir isdifadeci tapilmadi" });
            string token = Guid.NewGuid().ToString();

            user.ResetPassword = token;

            await _context.SaveChangesAsync();

            Message message = new Message(

                new List<string>() { email },

                "Warehouse Parolunu Sifirla",

                $"http://karfree-001-site1.atempurl.com/api/Auth/ResetPasswordConfirmation?token={user.ResetPassword}&&email={email}",

                null);

            await _emailSender.SendEmailAsync(message);
            return Ok(new Response<object>() { Message = "Növbəti addımlar üçün gələnlər qutusunu yoxlayın. Əgər Mail almamısınızsa və o spam qovluğunuzda deyilsə, bu başqa ünvanla qeydiyyatdan keçdiyiniz anlamına gələ bilər.." });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> ResetPasswordConfirmation([FromQuery] string token, string email, [FromQuery] string? newPassword, string? PasswordConfirmation)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();
            if (newPassword != PasswordConfirmation) return BadRequest(new { ErrorMessage = "Parollarinizi bir daha yaziniz" });
            if (user.ResetPassword != token) return BadRequest(new { SuccessMessage = "Bir xeta yarandi" });
            user.ResetPassword = null;
            await _context.SaveChangesAsync();


            if (newPassword == null) return Ok(new { Message = "Yeni Parolunuzu Giriniz" });
            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, newPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new { ErrorMessage = "Parolunuz Guncellenerken bir xeta bas verdi" });
            }
            return Ok(new { SuccessMessage = "Parolunuz Guncellendi" });

        }


    }
}
