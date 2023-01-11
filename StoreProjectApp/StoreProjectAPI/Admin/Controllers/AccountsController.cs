using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreProjectAPI.Admin.Dtos.AccountDtos;
using Store.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using StoreProjectAPI.Services;

namespace StoreProjectAPI.Admin.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Route("admin/api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _conf;
        private readonly IJwtService _jwtService;

        public AccountsController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration conf, IJwtService jwtService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _conf = conf;
            _jwtService = jwtService;
        }

        //[HttpGet("roles")]
        //public async Task<IActionResult> Create()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Member"));

        //    return Ok();
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            AppUser user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null || user.IsMember)
                return BadRequest();

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return BadRequest();
            var roles = await _userManager.GetRolesAsync(user);


            return Ok(new { token = _jwtService.GenerateToken(user, roles, _conf) });
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAdmin()
        {
            AppUser admin = new AppUser
            {
                UserName = "SuperAdmin",
                FullName = "Yusif Huseynzade",
            };

            await _userManager.CreateAsync(admin, "Admin123");
            await _userManager.AddToRoleAsync(admin, "SuperAdmin");

            return Ok();
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {

            return Ok(new { username = User.Identity.Name });
        }


    }

}

