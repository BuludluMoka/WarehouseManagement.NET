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


namespace Warehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly WarehouseDbContext _context;
        private readonly IMapper _mapper;

        public AuthController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, WarehouseDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _mapper = mapper;
        }


        [HttpPost, Route("Login")]

        public async Task<IActionResult> Login([FromForm] LoginDto model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(model.Email);
                //User deaktiv olunub
                //if (user.Status == false) { ModelState.AddModelError("ErrorMessage", "User Deaktiv olunub"); return BadRequest(ModelState); }
                if (user != null && user.Status != false &&await _userManager.CheckPasswordAsync(user,model.Password.Trim()))
                {
                    var roles = await _userManager.GetRolesAsync(user);
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
                        claims:claims,
                        signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                        );
                    return Ok(new
                    {
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

        [HttpPut("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromForm] UserUpdateDto model)
        {
            //AppUser currentUser = HttpContext.User.Identity as AppUser;
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            //var a = _userManager

            var checkPass = await _userManager.CheckPasswordAsync(currentUser, model.OldPassword);
            if (!checkPass) return BadRequest(new { ErrorMessage = "Girdiyiniz parol yanlisdir"});

            var token = await _userManager.GeneratePasswordResetTokenAsync(currentUser);
            

            var result = await _userManager.ResetPasswordAsync(currentUser, token, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { SuccessMessage = "Parol guncellendi"});
            }
            return BadRequest();
        }



    }
}
