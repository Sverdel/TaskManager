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
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System.Text;
using TaskManager.Core.Api.Filters;
using TaskManager.Core.Api.Hubs;
using TaskManager.Core.Api.Models.DataModel;
using TaskManager.TaskCore.Model;
using TaskManager.TaskCore.Repository;

namespace TaskManager.Core.Api
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
            string connectionString = Configuration["Data:DefaultConnection:ConnectionString"];
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
                           // укзывает, будет ли валидироваться издатель при валидации токена
                           ValidateIssuer = true,
                           // строка, представляющая издателя
                           ValidIssuer = Configuration["AppSettings:Issuer"],

                           // будет ли валидироваться потребитель токена
                           ValidateAudience = true,
                           // установка потребителя токена
                           ValidAudience = Configuration["AppSettings:SiteUrl"],
                           // будет ли валидироваться время существования
                           ValidateLifetime = true,

                           // установка ключа безопасности
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:SecurityKey"])),
                           // валидация ключа безопасности
                           ValidateIssuerSigningKey = true,
                       };
                       //})
                       //.AddFacebook(facebookOptions =>
                       //{
                       //    facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                       //    facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                       //    facebookOptions.SaveTokens = true;
                   });

            services.AddMvc()
                .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddAutoMapper();
            services.AddSignalR(config =>
            {
                config.JsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
            });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" }));
            services.ConfigureSwaggerGen(options => options.OperationFilter<AuthorizationOperationFilter>());

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

            app.UseSignalR(r => r.MapHub<TaskHub>("task"));

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
        }
    }
}
