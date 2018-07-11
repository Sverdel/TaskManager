using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Serilog;
using TaskManager.Api.Extensions;

namespace TaskManager.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var isService = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !Debugger.IsAttached && !args.Contains("--console");
            var contentRoot = isService
                    ? Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
                    : Directory.GetCurrentDirectory();

            var configuration = new ConfigurationBuilder()
               .SetBasePath(contentRoot)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
               .AddEnvironmentVariables()
               .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            var host = BuildWebHost(args.Where(arg => arg != "--console").ToArray(), contentRoot);

            try
            {
                Log.Information("Getting started...");

                if (isService)
                {
                    host.RunAsService();
                }
                else
                {
                    host.Run();
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost(string[] args, string contentRoot = null)
        {
            return WebHost.CreateDefaultBuilder<Startup>(args)
                          .UseContentRootSafe(contentRoot)
                          .UseUrls("http://*:54255/", "https://*:54256/")
                          .UseSerilog()
                          .Build();
        }
    }
}
