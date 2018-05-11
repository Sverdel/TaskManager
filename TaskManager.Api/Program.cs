using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace TaskManager.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                        .UseStartup<Startup>()
                        .UseUrls("http://localhost:54255/")
                        .Build();
        }
    }
}
