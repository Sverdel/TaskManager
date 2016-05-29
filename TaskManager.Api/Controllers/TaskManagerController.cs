using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskManager.Api.Models.DataModel;

namespace TaskManager.Api.Controllers
{
    [RoutePrefix("api/tasks")]
    public class TaskManagerController : ApiController
    {
        private TaskDbContext _context;

        public TaskManagerController()
        {
            _context = new TaskDbContext();
            
        }

        // GET api/<controller>
        [Route("{userId:int}")]
        public IHttpActionResult GetAll(int userId)
        {
            _context.Tasks.Add(new WorkTask { Id = 1, CreateDateTime = DateTime.Now, Name = "Temp task", UserId = userId });
            _context.SaveChanges();
            return Ok(_context.Tasks.ToList());//.Where(x => x.UserId == userId));
        }

        //[Route("{id:int}")]
        //// GET api/<controller>/5
        //public IHttpActionResult Get(int id)
        //{
        //    return Ok(_context.Tasks.Where(x => x.Id == id));
        //}

        //[Route]
        //[HttpPost]
        //// POST api/<controller>
        //public IHttpActionResult Post(WorkTask task)
        //{
        //    return Ok();
        //}

        //[Route("{id:int}")]
        //[HttpPut]
        //// PUT api/<controller>/5
        //public IHttpActionResult Put(int id, [FromBody]WorkTask task)
        //{
        //    return Ok();
        //}

        //[Route("{id:int}")]
        //[HttpDelete]
        //// DELETE api/<controller>/5
        //public IHttpActionResult Delete(int id)
        //{
        //    return Ok();
        //}
    }
}