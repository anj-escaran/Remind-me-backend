using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using RemindMe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace RemindMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Account> userManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<Account> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Register")]

        public async Task<IActionResult> Register([FromBody] Register model)
        {
            var userExist = await userManager.FindByNameAsync(model.Username);
            if (userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exist!" });
            Account user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username

            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed!" });
            }


            return Ok(new Response { Status = "Success", Message = "User Created Successfully" });
        }

        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await userManager.FindByNameAsync(model.Username);

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Secret").Value));

                var token = new JwtSecurityToken(
                    issuer: _configuration.GetSection("JWT:ValidIssuer").Value,
                    audience: _configuration.GetSection("JWT:ValidAudience").Value,
                    expires: DateTime.Now.AddHours(5),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    User = user.UserName,
                });
            }

            return Unauthorized();

        }

    }
}
