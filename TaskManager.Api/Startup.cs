using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using TaskManager.Api.Models.DataModel;
using System.Web.Http;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(TaskManager.Api.Startup))]

namespace TaskManager.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;
            //hubConfiguration.EnableJavaScriptProxies = false;
            app.MapSignalR("/api/signalr", hubConfiguration);

            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            //  Enable attribute based routing
            config.MapHttpAttributeRoutes();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.EnsureInitialized();
            app.UseWebApi(config);

        }
    }
}
