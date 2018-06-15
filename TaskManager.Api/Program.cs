using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;

namespace TaskManager.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var isService = !Debugger.IsAttached && !args.Contains("--console");

            var contentRoot = isService 
                ? Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
                : Directory.GetCurrentDirectory();

            var webHostArgs = args.Where(arg => arg != "--console").ToArray();

            var host = BuildWebHost(webHostArgs, contentRoot);

            if (isService)
            {
                host.RunAsService();
            }
            else
            {
                host.Run();
            }
        }

        public static IWebHost BuildWebHost(string[] args, string contentRoot)
        {
            return WebHost.CreateDefaultBuilder(args)
                        .UseStartup<Startup>()
                        .UseContentRoot(contentRoot)
                        .UseUrls("http://localhost:54255/")
                        .Build();
        }
    }
}
