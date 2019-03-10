using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IdentityAndTokenBasedAuthentication.Entities;
using IdentityAndTokenBasedAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAndTokenBasedAuthentication.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _Configuration;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private readonly CustomIdentityDbContext _Context;
        private readonly List<ProductDto> ProductList;

        public AuthController(IConfiguration configuration, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, CustomIdentityDbContext context)
        {
            _Configuration = configuration;
            _UserManager = userManager;
            _SignInManager = signInManager;
            _Context = context;

            ProductList = new List<ProductDto>();
        }

        [AllowAnonymous]
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] ApplicationUserDto item)
        {
            if (item == null)
            {
                return BadRequest("Model is null !"); // Bu model boş
            }

            var user = await _UserManager.FindByEmailAsync(item.Email) ?? await _UserManager.FindByNameAsync(item.UserName);

            if (user != null)
            {
                return BadRequest("This user is registered !"); // Bu Kullanıcı Kayıtlı
            }

            var jwtToken = GenerateJwtTokenBuilder();

            ApplicationUser appUser = new ApplicationUser()
            {
                UserName = item.UserName,
                Email = item.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                RefreshToken = GenerateRefreshToken(),
                Issued = DateTime.Now, // (Date of issue) veriliş tarihi
                Expired = DateTime.Now.AddMinutes(5),   //  (Date of expire) bitiş  tarihi
                Revoked = false

            };

            var result = await _UserManager.CreateAsync(appUser, item.Password);

            if (result.Succeeded)
            {
                return Ok(new TokenResponseDto { Token = jwtToken, RefreshToken = appUser.RefreshToken, AppUserId = appUser.Id, UserName = appUser.UserName, Email = appUser.Email });
            }

            return BadRequest();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] ApplicationUserDto item)
        {
            if (item == null)
            {
                return BadRequest("Model is null !");
            }

            var result = await _SignInManager.PasswordSignInAsync(item.UserName, item.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _UserManager.FindByEmailAsync(item.Email) ?? await _UserManager.FindByNameAsync(item.UserName);
                if (user != null)
                {
                    var jwtToken = GenerateJwtTokenBuilder();
                    return Ok(new TokenResponseDto { Token = jwtToken, RefreshToken = user.RefreshToken, AppUserId = user.Id, UserName = user.UserName, Email = user.Email });
                }
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpGet("GetRefreshToken")]
        public async Task<IActionResult> GetRefreshToken([FromBody] RefreshTokenInfoDto item)
        {
            if (item == null)
            {
                return BadRequest("Model is Null !");
            }

            var user = await _UserManager.FindByIdAsync(item.AppUserId);

            if (user != null)
            {
                if (user.RefreshToken == item.RefreshToken)
                {
                    var mydate = DateTime.Now;

                    if (user.Expired < mydate)
                    {
                        user.RefreshToken = GenerateRefreshToken();
                        user.Issued = DateTime.Now;  // (Date of issue) veriliş tarihi
                        user.Expired = DateTime.Now.AddMinutes(5);    //  (Date of expire) bitiş  tarihi
                        await _Context.SaveChangesAsync();

                        var jwtToken = GenerateJwtTokenBuilder();

                        return Ok(new TokenResponseDto { Token = jwtToken, RefreshToken = user.RefreshToken, AppUserId = user.Id, UserName = user.UserName, Email = user.Email });
                    }
                }
            }

            return BadRequest("Token has not expired"); // Token süresi dolmadı
        }

        // Token
        private string GenerateJwtTokenBuilder()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["JWT:key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: _Configuration["JWT:issuer"],     // (Pubslisher)      yayınlayıcı
                audience: _Configuration["JWT:audience"], // (Target)          hedef kitle
                signingCredentials: credentials,          // (Encryption type) şifreleme türü
                expires: DateTime.Now.AddMinutes(3)       // (Date of expire)  bitiş  tarihi
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        //Refresh Token
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}