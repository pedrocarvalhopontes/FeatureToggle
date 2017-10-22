using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToogleAPI.Models;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using ToogleAPI.Interface;
using ToogleAPI.DAL;

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
            //services.AddSingleton<DbContext, ToggleContext>(); //We need it to be singleton since we are using an in-memory database
            services.AddScoped<IRepository<Toggle>, ToggleRepository>();
            //services.AddScoped<IUnitOfWork<Toggle>, UnitOfWork<Toggle>>();
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Toggle API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Toggle API V1");
            });
        }
    }
}
