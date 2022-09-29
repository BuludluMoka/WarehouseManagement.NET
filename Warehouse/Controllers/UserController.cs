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
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly WarehouseDbContext _context;
        private readonly IMapper _mapper;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, WarehouseDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _mapper = mapper;
        }


        
        [HttpGet]
        public  IActionResult GetAllUsers()
        {
            List<AppUser> users = _userManager.Users.ToList();
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
        [Route("Login")]
        
        public async Task<IActionResult> Login([FromForm]LoginDto model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.CheckPasswordAsync(user,model.Password.Trim()))
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
                        issuer: "https://localhost:7199",
                        audience: "https://localhost:7199",
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


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] UserCreateDto userCreateDto)
        {
            if (ModelState.IsValid)
            {
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
                        if(roleResult.Succeeded)
                        {
                            _userManager.AddToRoleAsync(appUser, "User").Wait();
                        }

                    }
                    _userManager.AddToRoleAsync(appUser, "User").Wait();
                    return Ok();
                }
                else
                {
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                }
                
                
            }
            return BadRequest(ModelState);


        }

        [HttpPut("{id}")]
        public void UpdateUser(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void DeleteUser(int id)
        {
        }
        [HttpGet]
        [Route("GetInfo")]
        public async Task<IActionResult> getInfo(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }
       
    }
}
