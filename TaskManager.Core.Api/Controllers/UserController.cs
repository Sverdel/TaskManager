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

        public UserController(TaskDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet("{name}/{password}", Name = "GetUserRoute")]
        public async Task<IActionResult> GetUser(string name, string password)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Name == name);
            if (user == null)
            {
                return NotFound(new { message = "Invalid user name" });
            }

            if (user.Password != password)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, new { message = "Incorrect password" });
            }

            user.Token = Guid.NewGuid().ToString();

            return Ok(user);
        }

        [HttpPost("{name}/{password}")]
        public async Task<IActionResult> PostUser(string name, string password)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
            {
                return BadRequest(ModelState);
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Name == name);

            if (user != null)
            {
                return BadRequest("User with the same name already exists");
            }

            user = new User { Name = name, Password = password };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var newUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Name == name);
            newUser.Token = Guid.NewGuid().ToString();
            return CreatedAtRoute("GetUserRoute", null, newUser);
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