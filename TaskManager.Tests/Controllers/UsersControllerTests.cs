using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using TaskManager.Api.Models.DataModel;

namespace TaskManager.Api.Controllers.Tests
{
    [TestClass()]
    public class UsersControllerTests
    {
        [TestMethod()]
        public async Task GetUserTest()
        {
            UserController controller = new UserController();
            string name = "user1";
            string password = "12345";
            System.Web.Http.IHttpActionResult result = await controller.GetUser(name, password).ConfigureAwait(false);

            User user = (result as OkNegotiatedContentResult<User>)?.Content;

            Assert.IsNotNull(user);
            Assert.AreEqual(name, user.Name, "name");
            Assert.AreEqual(password, user.Password, "password");
            Assert.IsNotNull(user.Token, "token");
        }

        [TestMethod()]
        public async Task PostUserTest()
        {
            string name = "test";
            string password = "12345";
            try
            {
                UserController controller = new UserController();
                System.Web.Http.IHttpActionResult result = await controller.PostUser(name, password).ConfigureAwait(false);

                User user = (result as CreatedAtRouteNegotiatedContentResult<User>)?.Content;

                Assert.IsNotNull(user);
                Assert.AreEqual(name, user.Name, "name");
                Assert.AreEqual(password, user.Password, "password");
                Assert.IsNotNull(user.Token, "token");
            }
            finally
            {
                TaskDbContext context = new TaskDbContext();
                User user = context.Users.FirstOrDefault(x => x.Name == name);
                if (user != null)
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                }
            }
        }
    }
}