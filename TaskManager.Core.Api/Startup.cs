using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TaskManager.Core.Api.Infrastructure;
using TaskManager.Core.Api.Models.DataModel;
using TaskManager.Core.Api.Utils;

namespace TaskManager.Core.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new SignalRContractResolver()
            };

            var serializer = JsonSerializer.Create(settings);

            services.Add(new ServiceDescriptor(typeof(JsonSerializer),  provider => serializer,  ServiceLifetime.Transient));

            // Add framework services.
            services
                .AddMvc()
                .AddJsonOptions(opts => opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
                //.AddJsonOptions(opts => opts.SerializerSettings.ContractResolver = new SignalRContractResolver());

            services.AddSignalR(options => { options.Hubs.EnableDetailedErrors = true; });

            // Add Identity Services & Stores
            services.AddIdentity<User, IdentityRole>(config => {
                config.User.RequireUniqueEmail = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequiredLength = 1;
                config.Password.RequireLowercase = false;
                config.Password.RequireDigit = false;
                config.Cookies.ApplicationCookie.AutomaticChallenge = false;
            })
            .AddEntityFrameworkStores<TaskDbContext>()
            .AddDefaultTokenProviders();

            // Add ApplicationDbContext.
            services.AddDbContext<TaskDbContext>(options => options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            });

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            

            app.UseSignalR("/api/signalr");

            // Add a custom Jwt Provider to generate Tokens
            app.UseJwtProvider();
            // Add the Jwt Bearer Header Authentication to validate Tokens
            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                RequireHttpsMetadata = false,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = JwtProvider.SecurityKey,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = JwtProvider.Issuer,
                    ValidateIssuer = false,
                    ValidateAudience = false
                }
            });

            app.UseMvc();
        }
    }
}

