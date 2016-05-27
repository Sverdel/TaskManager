using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TaskManager.Api.Controllers
{
    [RoutePrefix("api/tasks")]
    public class TaskManagerController : ApiController
    {
        // GET api/<controller>
        [Route]
        public IHttpActionResult Get()
        {
            return Ok(new string[] { "value1", "value2" });
        }

        [Route]
        // GET api/<controller>/5
        public IHttpActionResult Get(int id)
        {
            return Ok("value");
        }

        [Route]
        [HttpPost]
        // POST api/<controller>
        public IHttpActionResult Post([FromBody]string value)
        {
            return Ok();
        }

        [Route]
        [HttpPut]
        // PUT api/<controller>/5
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            return Ok();
        }

        [Route]
        [HttpDelete]
        // DELETE api/<controller>/5
        public IHttpActionResult Delete(int id)
        {
            return Ok();
        }
    }
}