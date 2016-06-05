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
    public class UsersController : ApiController
    {
        private TaskDbContext _dbContext = new TaskDbContext();

        [Route()]
        public async Task<IHttpActionResult> GetUsers()
        {
            return Ok(_dbContext.Users.ToList().Select(x => x));
        }

        [Route("{name}/{password}", Name = "GetUserRoute")]
        public async Task<IHttpActionResult> GetUser(string name, string password)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Name == name);
            if (user == null)
            {
                return NotFound();
            }

            if (user.Password != password)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            return Ok(user);
        }

        [Route("{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> PutUser(int id, [FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(user).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_dbContext.Users.Count(e => e.Id == id) == 0)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
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
            return CreatedAtRoute("GetUserRoute", null, newUser);
        }

        [Route("{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return Ok(user);
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