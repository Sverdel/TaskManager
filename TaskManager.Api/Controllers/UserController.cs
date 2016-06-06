using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TaskManager.Api.Models.DataModel;

namespace TaskManager.Api.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private TaskDbContext _dbContext = new TaskDbContext();

        [Route("{name}/{password}", Name = "GetUserRoute")]
        public async Task<IHttpActionResult> GetUser(string name, string password)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Name == name);
            if (user == null)
            {
                return Content(HttpStatusCode.NotFound, new { Mmessage = "Invalid user name" });
            }

            if (user.Password != password)
            {
                return Content(HttpStatusCode.Unauthorized, new { Mmessage = "Incorrect password" });
            }

            user.Token = Guid.NewGuid().ToString();

            return Ok(user);
        }

        [Route("{name}/{password}")]
        [HttpPost]
        public async Task<IHttpActionResult> PostUser(string name, string password)
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