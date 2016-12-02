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

        [HttpGet("states")]
        public async Task<IActionResult> GetStates()
        {
            return Ok(_dbContext.States.ToList());
        }

        [HttpGet("priorities")]
        public async Task<IActionResult> GetPriorities()
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
