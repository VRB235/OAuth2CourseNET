using IdentityExample.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<AppDbContext>(config => // Aguega una BD
            {
                config.UseInMemoryDatabase("Memory"); // Agrega la BD a memoria
            });

            // Registra los servicios o infraestructura
            services.AddIdentity<IdentityUser, IdentityRole>(config => {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<AppDbContext>() 
                .AddDefaultTokenProviders(); // Provee un token por default

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookie"; // Nombre de la Cookie
                config.LoginPath = "/Home/Login"; // Ruta de autenticación
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting(); // Mira la ruta a la que se accesa

            app.UseAuthentication(); // Cheque la identidad del usuario

            app.UseAuthorization(); // Chequea la autorización del usuario

            app.UseEndpoints(endpoints => // Utiliza el endpoint al que se accesa
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
