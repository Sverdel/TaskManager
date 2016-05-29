using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace TaskManager.Api
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "AllTasks",
                routeTemplate: "tasks",
                defaults: new { controller = "TaskManager", action = "GetAll" }
                );
        }
    }
}