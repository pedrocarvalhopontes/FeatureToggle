﻿using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using ToggleAPI.Mapping;
using ToggleAPI.Models;
using ToggleAPI.Repository;

namespace ToggleAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public ILoggerFactory LoggerFactory { get; }

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ToggleContext>(opt => opt.UseInMemoryDatabase("Toggle API"));
            services.AddScoped<IRepository<Toggle>, ToggleRepository>();
            services.AddScoped<IToggleRepository, ToggleRepository>();
            AddAutheticationAndAuthorization(services);

            services.AddMvc(setupAction =>{ setupAction.ReturnHttpNotAcceptable = true; });
            
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "Toggle API", Version = "v1" }); });
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ToggleContext context, UserManager<SystemUser> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            LoggerFactory.AddDebug();
            SetupExceptionHandler(app, env);
            app.UseMvc();
            app.UseAuthentication();
            SetupAutomapper();
            SetupSwaggerDocumentation(app);

            context.EnsureSeedDataWithUsers(userMgr, roleMgr);
        }

        private void AddAutheticationAndAuthorization(IServiceCollection services)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = Configuration["Tokens:Issuer"],
                ValidAudience = Configuration["Tokens:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                ValidateLifetime = true
            };
            var logger = LoggerFactory.CreateLogger("Jwt events logger");
            var tokenEvents = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    logger.LogInformation("OnAuthenticationFailed: " +
                                      context.Exception.Message);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    logger.LogInformation("OnTokenValidated: " +
                                      context.SecurityToken);
                    return Task.CompletedTask;
                }
                
            };

            services.AddIdentity<SystemUser, IdentityRole>()
                .AddEntityFrameworkStores<ToggleContext>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Audience = Configuration["Tokens:Audience"];
                o.TokenValidationParameters = tokenValidationParameters;
                o.Events = tokenEvents;
            });

            services.AddAuthorization(cfg =>
             {
                 cfg.AddPolicy("Administrators", p => p.RequireClaim("SuperUser", "True"));
             });
        }

        private void SetupExceptionHandler(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder => appBuilder.Run(
                    async ctx =>
                    {
                        var exception = ctx.Features.Get<IExceptionHandlerFeature>();
                        if (exception != null)
                        {
                            var logger = LoggerFactory.CreateLogger("Global Exception logger");
                            logger.LogError(500,  exception.Error, exception.Error.Message);
                        }

                        ctx.Response.StatusCode = 500;
                        await ctx.Response.WriteAsync("An error occured. Please try again later.");
                    }));
            }
        }

        private static void SetupSwaggerDocumentation(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Toggle API V1");
            });
        }

        private static void SetupAutomapper()
        {
            var cfg = new ToggleMappingConfiguration();
            AutoMapper.Mapper.Initialize(cfg.ConfigurationAction);
        }
    }
}
