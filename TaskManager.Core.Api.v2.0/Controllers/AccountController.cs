using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Api.Models.DataModel;
using TaskManager.Core.Api.Models.Dto;

namespace TaskManager.Core.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private IConfiguration _config;
        private UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager, IConfiguration config)
        {
            _config = config;
            _userManager = userManager;
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody]CredentialsDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var user = await _userManager.FindByNameAsync(model.Name);

                if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    return BadRequest();
                }

                var token = await GetJwtSecurityToken(user);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private async Task<JwtSecurityToken> GetJwtSecurityToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            DateTime now = DateTime.UtcNow;

            return new JwtSecurityToken(
                issuer: _config["AppSettings:Issuer"],
                audience: _config["AppSettings:SiteUrl"],
                claims: GetTokenClaims(user, now).Union(userClaims),
                expires: now.AddMinutes(int.Parse(_config["AppSettings:Lifetime"])),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AppSettings:SecurityKey"])), SecurityAlgorithms.HmacSha256)
            );
        }

        private IEnumerable<Claim> GetTokenClaims(User user, DateTime now)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Iss, _config["AppSettings:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new  DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };
        }
    }
}