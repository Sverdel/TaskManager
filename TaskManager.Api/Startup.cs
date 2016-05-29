using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using TaskManager.Api.Models.DataModel;
using System.Web.Http;

[assembly: OwinStartup(typeof(TaskManager.Api.Startup))]

namespace TaskManager.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            //  Enable attribute based routing
            config.MapHttpAttributeRoutes();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            app.UseWebApi(config);
        }
    }
}
