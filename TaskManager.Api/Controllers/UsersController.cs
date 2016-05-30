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
using TaskManager.Api.Models.Dto;

namespace TaskManager.Api.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private TaskDbContext _dbContext = new TaskDbContext();

        private Func<UserDto, User> FromDto = dto => new User { Id = dto.Id, Name = dto.Name };
        private Func<User, UserDto> ToDto = user => new UserDto { Id = user.Id, Name = user.Name };


        [Route("", Name = "GetUserRoute")]
        public async Task<IHttpActionResult> GetUsers()
        {
            return Ok(_dbContext.Users.ToList().Select(x => ToDto(x)));
        }

        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(ToDto(user));
        }

        [Route("{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> PutUser(int id, UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = FromDto(userDto);
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

        [Route]
        [HttpPost]
        public async Task<IHttpActionResult> PostUser(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = FromDto(userDto);
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return CreatedAtRoute("GetUserRoute", null, user);
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

            return Ok(ToDto(user));
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