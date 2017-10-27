using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToogleAPI.Models;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using ToogleAPI.Interface;
using ToogleAPI.DAL;
using Microsoft.AspNetCore.Http;
using ToggleAPI.Mapping;

namespace ToogleAPI
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
            services.AddDbContext<ToggleContext>(opt => opt.UseInMemoryDatabase("Toggle API"));
            services.AddScoped<IRepository<Toggle>, ToggleRepository>();
            services.AddMvc(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                //Note to self: Extra output formatters can be added here
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Toggle API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ToggleContext context)
        {
            //TODO:refator exception handling
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder => appBuilder.Run(
                    async ctx =>
                    {
                        ctx.Response.StatusCode = 500;
                        await ctx.Response.WriteAsync("An error occured. Please try again later.");
                    }));
            }

            app.UseMvc();
            SetupSwaggerDocumentation(app);
            SetupAutomapper();

            //Seeding context data for Demo purposes
            context.EnsureSeedDataForContext();
        }

        private static void SetupSwaggerDocumentation(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Toggle API V1"); });
        }

        private static void SetupAutomapper()
        {
            var cfg = new ToggleMappingConfiguration();
            AutoMapper.Mapper.Initialize(cfg.ConfigurationAction);
        }
    }
}
