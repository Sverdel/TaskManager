using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using TaskManager.Api.Hubs;
using TaskManager.Api.Models.Configs;
using TaskManager.Api.Models.DataModel;
using TaskManager.Core.Model;
using TaskManager.Core.Repository;

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
            string connectionString = Configuration["Data:DefaultConnection:ConnectionString"];
            services.AddTransient<IRepository<WorkTask, long>, TaskRepository>(serv => new TaskRepository(connectionString));
            services.AddTransient<IRepository<State, int>, StateRepository>(serv => new StateRepository(connectionString));
            services.AddTransient<IRepository<Priority, int>, PriorityRepository>(serv => new PriorityRepository(connectionString));
            services.AddSingleton<IAccountConfig, AccountConfig>(serv => accountConfig);

            services.AddDbContext<TaskDbContext>(options => options.UseSqlServer(connectionString));

            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.RequireHttpsMetadata = false;
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           // укзывает, будет ли валидироваться издатель при валидации токена
                           ValidateIssuer = true,
                           // строка, представляющая издателя
                           ValidIssuer = accountConfig.Issuer,

                           // будет ли валидироваться потребитель токена
                           ValidateAudience = true,
                           // установка потребителя токена
                           ValidAudience = accountConfig.SiteUrl,
                           // будет ли валидироваться время существования
                           ValidateLifetime = true,

                           // установка ключа безопасности
                           IssuerSigningKey = accountConfig.Key,
                           // валидация ключа безопасности
                           ValidateIssuerSigningKey = true,
                       };
                   });

            services.AddMvc()
                .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddAutoMapper();
            services.AddSignalR();

            // Register the Swagger generator, defining one or more Swagger documents
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

            // Add Identity Services & Stores
            services.AddIdentity<User, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequiredLength = 1;
                config.Password.RequireLowercase = false;
                config.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<TaskDbContext>()
            .AddDefaultTokenProviders();

            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();

            app.UseSignalR(r => r.MapHub<TaskHub>("/task"));

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
        }
    }
}
