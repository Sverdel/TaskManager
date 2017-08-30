using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using TaskManager.Api.Models.DataModel;

namespace TaskManager.Api.Controllers.Tests
{
    [TestClass()]
    public class ResourceControllerTests
    {
        [TestMethod()]
        public async Task GetStatesTest()
        {
            ResourceController controller = new ResourceController();
            System.Web.Http.IHttpActionResult result = await controller.GetStates();

            List<State> states = (result as OkNegotiatedContentResult<List<State>>).Content;

            Assert.IsNotNull(states);
            Assert.AreEqual(3, states.Count);
        }

        [TestMethod()]
        public async Task GetPrioritiesTest()
        {
            ResourceController controller = new ResourceController();
            System.Web.Http.IHttpActionResult result = await controller.GetPriorities();

            List<Priority> priorities = (result as OkNegotiatedContentResult<List<Priority>>).Content;

            Assert.IsNotNull(priorities);
            Assert.AreEqual(3, priorities.Count);
        }
    }
}