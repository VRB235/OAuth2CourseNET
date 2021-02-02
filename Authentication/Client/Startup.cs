using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client
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
            services.AddAuthentication(config => {
                config.DefaultAuthenticateScheme = "ClientCookie"; // Confirma que está autenticado
                config.DefaultSignInScheme = "ClientCookie"; // Cuando se autentica se repartirá una cookie
                config.DefaultChallengeScheme = "OurServer"; // Verifica si tiene permitido realizar acciones
            })
            .AddCookie("ClientCookie")
            .AddOAuth("OurServer",config => {
                config.CallbackPath = "/oauth/callback";
                config.ClientId = "client_id";
                config.ClientSecret = "client_secret";
                config.AuthorizationEndpoint = "https://localhost:44390/oauth/authorize";
                config.TokenEndpoint = "https://localhost:44390/oauth/token";
                config.SaveTokens = true;
                config.Events = new OAuthEvents()
                {
                    OnCreatingTicket = context =>
                    {
                        var accessToken = context.AccessToken;
                        var base64Payload = accessToken.Split(".")[1];
                        var bytes = Convert.FromBase64String(base64Payload);
                        var jsonPayload = System.Text.Encoding.UTF8.GetString(bytes);
                        var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPayload);
                        foreach (var claim in claims)
                        {
                            context.Identity.AddClaim(new Claim(claim.Key, claim.Value));
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
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
