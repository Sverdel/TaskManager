using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using TaskManager.Api.Models.DataModel;

namespace TaskManager.Api.Controllers
{
    [RoutePrefix("api/resources")]
    public class ResourceController : ApiController
    {
        private readonly TaskDbContext _dbContext = new TaskDbContext();

        [Route("states")]
        public async Task<IHttpActionResult> GetStates()
        {
            return Ok(_dbContext.States.ToList());
        }

        [Route("priorities")]
        public async Task<IHttpActionResult> GetPriorities()
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
