using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProyectoBase.Models;
using ProyectoBase.Pages;


namespace ProyectoBase
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("LibraryConnection")));
            services.AddMvc()
             .AddRazorPagesOptions(options =>
              {
                  options.RootDirectory = "/Pages/Menu";
                  options.Conventions.AuthorizeFolder("/Menu");
              });
            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            });
            // Add application services.  
            //services.AddTransient<IEmailSender, EmailSender>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
              //  app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            //app.UseAuthentication();
            //  app.UseMvc();
            app.UseMvc(routes =>
            {
              //  routes.MapRoute(
                //    name: "default",
                    //template: "{controller=Pages}/{controller=Menu}/{action=Index}/{id?}");
                  //  template: "Pages/Menu/FicPersonas");
            });
        }
    }
}
