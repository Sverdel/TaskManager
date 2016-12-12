using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading.Tasks;
using TaskManager.Core.Api.Models.DataModel;

namespace TaskManager.Core.Api.Controllers
{
    [Route("api/users")]
    public class UserController : Controller
    {
        private TaskDbContext _dbContext;
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private SignInManager<User> _signinManager;
        private string _role = "User";

        public UserController(TaskDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signinManager)
        {
            _dbContext = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signinManager = signinManager;
            //if (!_roleManager.RoleExistsAsync(_role).Result)
            //{
            //    _roleManager.CreateAsync(new IdentityRole(_role)).Wait();
            //}
        }

        [HttpGet("{name}/{password}", Name = "GetUserRoute")]
        public async Task<IActionResult> GetUser(string name, string password)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == name);
            if (user == null)
            {
                return NotFound(new { message = "Invalid user name" });
            }

            if (user.PasswordHash != password)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, new { message = "Incorrect password" });
            }

            user.Token = Guid.NewGuid().ToString();

            return Ok(user);
        }

        //[HttpPost("{name}/{password}")]
        //public async Task<IActionResult> PostUser(string name, string password)
        //{
        //    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == name);

        //    if (user != null)
        //    {
        //        return BadRequest("User with the same name already exists");
        //    }

        //    user = new User { UserName = name, PasswordHash = password };

        //    _dbContext.Users.Add(user);
        //    await _dbContext.SaveChangesAsync();

        //    var newUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == name);
        //    newUser.Token = Guid.NewGuid().ToString();
        //    return CreatedAtRoute("GetUserRoute", null, newUser);
        //}

        /// <summary>
        /// POST: api/accounts
        /// </summary>
        /// <returns>Creates a new User and return it accordingly.</returns>
        [HttpPost("{name}/{password}")]
        public async Task<IActionResult> Add(string name, string password)
        {

            try
            {
                // check if the Username/Email already exists
                User user = await _userManager.FindByNameAsync(name);
                if (user != null)
                {
                    throw new Exception("UserName already  exists.");
                }

                user = new User()
                {
                    UserName = name,
                };
                // Add the user to the Db with a random password
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.ToString());
                }

                // Assign the user to the 'Registered' role.
                result = await _userManager.AddToRoleAsync(user, _role.ToUpper());
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.ToString());
                }

                // Remove Lockout and E-Mail confirmation
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;

                //_dbContext.Users.Add(user);

                _dbContext.SaveChanges();

                return Ok(new
                {
                    id = user.Id,
                    name = user.UserName,
                    password = password,
                    token = Guid.NewGuid().ToString()
                });
            }
            catch (Exception e)
            {
                // return the error.
                return StatusCode((int)HttpStatusCode.BadRequest, new { error = e.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await _signinManager.SignOutAsync();
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}