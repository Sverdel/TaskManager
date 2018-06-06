using AutoMapper;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.Api.Models.Configs;
using TaskManager.Api.Models.DataModel;
using TaskManager.Api.Models.Dto;

namespace TaskManager.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountConfig _config;
        private readonly UserManager<User> _userManager;
        private readonly TaskDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signinManager;
        private readonly string _role = "User";

        public AccountController(TaskDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signinManager, IAccountConfig config)
        {
            _config = config;
            _userManager = userManager;
            _dbContext = context;
            _roleManager = roleManager;
            _signinManager = signinManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("signin")]
        public async Task<ActionResult<UserDto>> SignIn(CredentialsDto model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Name).ConfigureAwait(false);

                if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password).ConfigureAwait(false))
                {
                    return BadRequest("Incorrect user name or password");
                }

                return await CreateUserDto(user).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new User and return it accordingly.
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost("signup")]
        public async Task<ActionResult<UserDto>> SignUp(UserDto userModel)
        {
            try
            {
                // check if the Username/Email already exists
                var user = await _userManager.FindByNameAsync(userModel.UserName).ConfigureAwait(false);
                if (user != null)
                {
                    return BadRequest("User name is already exists.");
                }

                user = Mapper.Map<User>(userModel);
                var errors = await CreateUser(user, userModel.Password).ConfigureAwait(false);
                if (errors != null)
                {
                    return BadRequest(errors);
                }

                return await CreateUserDto(user).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                // return the error.
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("signout")]
        public async Task<IActionResult> SignOut()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await _signinManager.SignOutAsync().ConfigureAwait(false);
            }

            return Ok();
        }

        [HttpPost("signinExt/{provider?}")]
        public IActionResult ExternalLogin(string provider = FacebookDefaults.AuthenticationScheme)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account");
            var properties = _signinManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            
            return Challenge(properties, provider);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("signinCallback")]
        public async Task<ActionResult<UserDto>> ExternalLoginCallback(string remoteError = null)
        {
            if (remoteError != null)
            {
                return BadRequest($"Error from external provider: {remoteError}");
            }

            var info = await _signinManager.GetExternalLoginInfoAsync().ConfigureAwait(false);
            if (info == null)
            {
                return BadRequest("Can't signin using external service");
            }

            var result = await _signinManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            var user = Models.DataModel.User.FromIdentity(info.Principal);
            if ((await _userManager.FindByEmailAsync(user.Email).ConfigureAwait(false)) == null)
            {
                var errors = await CreateUser(user, Guid.NewGuid().ToString()).ConfigureAwait(false);
                if (errors != null)
                {
                    return BadRequest(errors);
                }
            }

            return await CreateUserDto(user).ConfigureAwait(false);
        }

        private async Task<IEnumerable<IdentityError>> CreateUser(User user, string password)
        {
            // Add the user to the Db with a random password
            var result = await _userManager.CreateAsync(user, password).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return result.Errors;
            }

            // Assign the user to the 'Registered' role.
            result = await _userManager.AddToRoleAsync(user, _role.ToUpper()).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return result.Errors;
            }

            // Remove Lockout and E-Mail confirmation
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;

            _dbContext.SaveChanges();

            return null;
        }

        private async Task<UserDto> CreateUserDto(User user)
        {
            var token = await GetJwtSecurityToken(user).ConfigureAwait(false);

            var userDto = Mapper.Map<UserDto>(user);
            userDto.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
            userDto.TokenExpireDate = token.ValidTo;

            return userDto;
        }

        private async Task<JwtSecurityToken> GetJwtSecurityToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);

            var now = DateTime.UtcNow;

            return new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.SiteUrl,
                claims: GetTokenClaims(user, now).Union(userClaims),
                expires: now.AddMinutes(_config.LifetimeMinutes),
                signingCredentials: new SigningCredentials(_config.Key, SecurityAlgorithms.HmacSha256)
            );
        }

        private IEnumerable<Claim> GetTokenClaims(User user, DateTime now)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Iss,  _config.Issuer),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new  DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };
        }
    }
}