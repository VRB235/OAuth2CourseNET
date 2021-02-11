using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var Scope = host.Services.CreateScope())
            {
                var userManager = Scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                var user = new IdentityUser("bob");

                userManager.CreateAsync(user, "password").GetAwaiter().GetResult();

                userManager.AddClaimAsync(user, new Claim("rc.grandma", "big.cookie")).GetAwaiter().GetResult();
                userManager.AddClaimAsync(user, new Claim("rc.api.grandma", "big.api.cookie")).GetAwaiter().GetResult();

                //Scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                //var context = Scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                //context.Database.Migrate();
                //if (!context.Clients.Any())
                //{
                //    foreach (var client in Configuration.GetClients())
                //    {
                //        context.Clients.Add(client.ToEntity());
                //    }
                //    context.SaveChanges();
                //}

                //if (!context.IdentityResources.Any())
                //{
                //    foreach (var resource in Configuration.GetIdentityResources())
                //    {
                //        context.IdentityResources.Add(resource.ToEntity());
                //    }
                //    context.SaveChanges();
                //}

                //if (!context.ApiScopes.Any())
                //{
                //    foreach (var resource in Configuration.GetScopes())
                //    {
                //        context.ApiScopes.Add(resource.ToEntity());
                //    }
                //    context.SaveChanges();
                //}

                //if(!context.ApiResources.Any())
                //{
                //    foreach (var resource in Configuration.GetApis())
                //    {
                //        context.ApiResources.Add(resource.ToEntity());
                //    }
                //    context.SaveChanges();
                //}
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
