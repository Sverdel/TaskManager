using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TaskManager.Api.Startup))]

namespace TaskManager.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            app.Run(context =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                return context.Response.WriteAsync("<h2>Привет мир!</h2>");
            });
        }
    }
}
