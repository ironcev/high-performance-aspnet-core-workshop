using GettingThingsDone.ApplicationCore.Services;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Infrastructure.Database;
using GettingThingsDone.WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GettingThingsDone.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var inMemoryDatabaseRoot = new InMemoryDatabaseRoot();
            services.AddDbContext<GettingThingsDoneDbContext>(options => options.UseInMemoryDatabase("GettingThingsDoneDatabase", inMemoryDatabaseRoot));

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfAsyncRepository<>));
            services.AddScoped<IActionService, ActionService>();
            services.AddScoped<IActionListService, ActionListService>();
            services.AddScoped<IProjectService, ProjectService>();

            //add authentication JWT options settings
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "gettingthingsdone.com",
                        ValidAudience = "gettingthingsdone.com",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))
                    };
                });
            //add custom authorization claim based policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("OlderOnly",
                    policy => policy
                    .RequireClaim(CustomClaimTypes.Editor)
                    .AddRequirements(new OlderThenRequirement(50)));
            });
            //register OlderThen Handler
            services.AddSingleton<IAuthorizationHandler, OlderThenHandler>();
            //versioning
            services.AddApiVersioning(v =>
            {
                v.AssumeDefaultVersionWhenUnspecified = true;
                v.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //add authentication - without iz for example [Authorize] want work
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}