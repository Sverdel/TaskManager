using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using TaskManager.Api.Hubs;
using TaskManager.Api.Models.Configs;
using TaskManager.Api.Models.DatabaseContext;
using TaskManager.Api.Models.DataModel;
using TaskManager.Api.Services;

namespace TaskManager.Api
{
    public class Startup
    {
        private readonly IServiceProvider provider;

        public Startup(IConfiguration configuration, IServiceProvider provider)
        {
            Configuration = configuration;
            this.provider = provider;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var accountConfig = new AccountConfig(Configuration);
            var connectionString = Configuration["Data:DefaultConnection:ConnectionString"];
            services.AddCors();
            Core.Startup.ConfigureServices(services, connectionString);
            services.AddSingleton<IAccountConfig>(serv => accountConfig);
            services.AddSingleton<IHostedService, ExchangeService>();

            services.AddDbContext<TaskIdentityContext>(options => options.UseSqlServer(connectionString));

            
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

                    })
                    .AddLinkedIn(options =>
                    {
                        options.ClientId = Configuration["Authentication:LinkedIn:AppId"];
                        options.ClientSecret = Configuration["Authentication:LinkedIn:AppSecret"];
                    });

            services.AddMvc()
                    .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Startup>())
                    .SetCompatibilityVersion(CompatibilityVersion.Latest);
            
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
            .AddEntityFrameworkStores<TaskIdentityContext>()
            .AddDefaultTokenProviders();

            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();

            app.UseSignalR(r =>
            {
                r.MapHub<TaskHub>("/task");
                r.MapHub<ExchangeHub>("/exchange");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
        }
    }
}
