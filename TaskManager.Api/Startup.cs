using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using TaskManager.Api.Models.DataModel;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json;
using TaskManager.Api.Utils;
using Newtonsoft.Json.Converters;

[assembly: OwinStartup(typeof(TaskManager.Api.Startup))]

namespace TaskManager.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            // set serializer to signalr
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();
            var serializer = JsonSerializer.Create(settings);
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);

            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;
            app.MapSignalR("/api/signalr", hubConfiguration);

            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            //  Enable attribute based routing
            config.MapHttpAttributeRoutes();

            //Delete all formatter except of json
            var jsonFormatter = config.Formatters.JsonFormatter;
            config.Formatters.Clear();
            config.Formatters.Add(jsonFormatter);

            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.EnsureInitialized();

            app.UseWebApi(config);

        }
    }

    
}
