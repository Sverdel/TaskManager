using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Core.Api.Models.DataModel;

namespace TaskManager.Core.Api.Controllers
{
    [Route("api/resources")]
    public class ResourceController : Controller
    {
        private TaskDbContext _dbContext;

        public ResourceController(TaskDbContext context)
        {
            _dbContext = context;
        }

        [Authorize]
        [HttpGet("states")]
        public IActionResult GetStates()
        {
            return Ok(_dbContext.States.ToList());
        }

        [Authorize]
        [HttpGet("priorities")]
        public IActionResult GetPriorities()
        {
            return Ok(_dbContext.Priorities.ToList());
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
