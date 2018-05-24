using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Api.Hubs;
using TaskManager.Api.Models.DataModel;
using TaskManager.Core.Model;
using TaskManager.Core.Repository;

namespace TaskManager.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c => c.AddPolicy("cors", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

            var connectionString = Configuration["Data:DefaultConnection:ConnectionString"];
            services.AddTransient<IRepository<WorkTask, long>, TaskRepository>(serv => new TaskRepository(connectionString));
            services.AddTransient<IRepository<State, int>, StateRepository>(serv => new StateRepository(connectionString));
            services.AddTransient<IRepository<Priority, int>, PriorityRepository>(serv => new PriorityRepository(connectionString));

            services.AddDbContext<TaskDbContext>(options => options.UseSqlServer(connectionString));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = Configuration["AppSettings:Issuer"],
                            ValidateAudience = true,
                            ValidAudience = Configuration["AppSettings:SiteUrl"],
                            ValidateLifetime = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:SecurityKey"])),
                            ValidateIssuerSigningKey = true,
                        };
                    })
                    .AddFacebook(facebookOptions =>
                    {
                        facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                        facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                        facebookOptions.SaveTokens = true;

                    }).AddLinkedIn(options =>
                    {
                        options.ClientId = Configuration["Authentication:LinkedIn:AppId"];
                        options.ClientSecret = Configuration["Authentication:LinkedIn:AppSecret"];
                    });

            services.AddMvc()
                    .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddAutoMapper();
            services.AddSignalR();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                });

            });

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

            //    c.AddSecurityDefinition("oauth2", new OAuth2Scheme
            //    {
            //        Flow = "implicit",
            //        AuthorizationUrl = "https://www.facebook.com/v2.6/dialog/oauth",
            //        //Scopes = new Dictionary<string, string> {
            //        //    { "public_profile", "public_profile"},
            //        //    { "email", "email" }
            //        //},
            //        //TokenUrl = "/signin-facebook"
            //    });
            //});

            services.AddIdentity<User, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = false;
                config.User.AllowedUserNameCharacters += " ";
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequiredLength = 0;
                config.Password.RequireLowercase = false;
                config.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<TaskDbContext>()
            .AddDefaultTokenProviders();

            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            //app.UseExceptionHandler(builder =>
            //{
            //    builder.Run(async context =>
            //    {
            //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            //        var error = context.Features.Get<IExceptionHandlerFeature>();
            //        if (error != null)
            //        {
            //            context.Response.Headers.Add("Application-Error", error.Error.Message);
            //            // CORS
            //            context.Response.Headers.Add("access-control-expose-headers", "Application-Error");
            //            await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
            //        }
            //    });
            //});
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();
            //app.UseMvc(routes =>
            //{
            //    //routes.MapRoute(name: "signin-facebook", template: "signin-facebook", defaults: new { controller = "Account", action = "ExternalLoginCallback" });
            //});

            app.UseSignalR(r => r.MapHub<TaskHub>("/task"));

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

            //    c.OAuthAppName("TestAuth");
            //    c.OAuthClientId(Configuration["Authentication:Facebook:AppId"]);
            //    c.OAuthClientSecret(Configuration["Authentication:Facebook:AppSecret"]);
            //    c.OAuth2RedirectUrl("http://localhost:54255/signin-facebook");
            //    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            //});
        }
    }
}
