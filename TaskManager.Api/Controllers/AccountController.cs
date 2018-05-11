using AutoMapper;
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
using TaskManager.Api.Models.DataModel;
using TaskManager.Api.Models.Dto;

namespace TaskManager.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly TaskDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signinManager;
        private readonly string _role = "User";

        public AccountController(TaskDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signinManager, IConfiguration config)
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
        public async Task<IActionResult> SignIn([FromBody]CredentialsDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                User user = await _userManager.FindByNameAsync(model.Name).ConfigureAwait(false);

                if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password).ConfigureAwait(false))
                {
                    return BadRequest();
                }

                JwtSecurityToken token = await GetJwtSecurityToken(user).ConfigureAwait(false);

                UserDto userDto = Mapper.Map<User, UserDto>(user);
                userDto.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
                userDto.TokenExpireDate = token.ValidTo;

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// Creates a new User and return it accordingly.
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody]UserDto userModel)
        {
            try
            {
                // check if the Username/Email already exists
                User user = await _userManager.FindByNameAsync(userModel.UserName).ConfigureAwait(false);
                if (user != null)
                {
                    return BadRequest("User name is already exists.");
                }

                user = Mapper.Map<UserDto, User>(userModel);
                // Add the user to the Db with a random password
                IdentityResult result = await _userManager.CreateAsync(user, userModel.Password).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                // Assign the user to the 'Registered' role.
                result = await _userManager.AddToRoleAsync(user, _role.ToUpper()).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                // Remove Lockout and E-Mail confirmation
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;

                _dbContext.SaveChanges();

                return Ok(Mapper.Map<User, UserDto>(user));
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

        //[HttpGet]
        //[Route("regExternal")]
        //public async Task<IActionResult> Register(string provider, string error = null)
        //{
        //    return await ExternalLogin(provider, true, error);
        //}

        //[HttpPost]
        //[Route("loginExternal")]
        //public async Task<IActionResult> Login(string provider, string error = null)
        //{
        //    return await ExternalLogin(provider, false, error);
        //}

        ///// <summary>
        ///// Register/Login via external oauth service
        ///// </summary>
        ///// <param name="provider"></param>
        ///// <param name="register"></param>
        ///// <param name="error"></param>
        ///// <returns></returns>
        //private async Task<IActionResult> ExternalLogin(string provider, bool register, string error = null)
        //{
        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return new ChallengeResult(provider);
        //    }

        //    var user = Models.DataModel.User.FromIdentity(User.Identity as ClaimsIdentity);

        //    if (user == null)
        //    {
        //        return NoContent();
        //    }

        //    if (user.LoginProvider != provider)
        //    {
        //        await _signinManager.SignOutAsync();
        //        return new ChallengeResult(provider);
        //    }

        //    bool userExists = (await _userManager.FindByEmailAsync(user.Email)) != null;

        //    if (register)
        //    {
        //        if (!userExists)
        //        {
        //            return BadRequest("User already exists");
        //        }

        //        await _userManager.CreateAsync(user);
        //    }
        //    else
        //    {
        //        if (userExists)
        //        {
        //            return BadRequest("User already exists");
        //        }
        //    }

        //    return Ok("Welcome, " + user.UserName);
        //}

        private async Task<JwtSecurityToken> GetJwtSecurityToken(User user)
        {
            IList<Claim> userClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);

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