using IdentityTest.Data;
using IdentityTest.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            //CreateWebHostBuilder(args).Build().Run();

            using(var scope = host.Services.CreateScope())
            {
                try
                {
                    DocumentDBConfig.Configure();

                    var services = scope.ServiceProvider;
                    var serviceProvider = services.GetRequiredService<IServiceProvider>();
                    var configuration = services.GetRequiredService<IConfiguration>();
                    CreateRoles(serviceProvider).Wait();
                    CreateAdminAccount(serviceProvider, configuration).Wait();
                }
                catch (Exception e)
                {
                    return;
                }

            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //adding customs roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
           
            var roleNames = new List<string>(){ "Admin", "Member" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                // creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var identityRole = new IdentityRole(roleName);
                    roleResult = await RoleManager.CreateAsync(identityRole);
                }
            }
        }

        private static async Task CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var useEf = false;

            if (!useEf)
            {
                var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var administrator = new IdentityUser
                {
                    UserName = configuration.GetSection("UserSettings")["UserEmail"],
                    Email = configuration.GetSection("UserSettings")["UserEmail"]
                };

                var userPassword = configuration.GetSection("UserSettings")["UserPassword"];
                var user = await UserManager.FindByEmailAsync(administrator.Email);

                if (user == null)
                {
                    var createPowerUser = await UserManager.CreateAsync(administrator, userPassword);
                    if (createPowerUser.Succeeded)
                    {
                        // here we assign the new user the "Admin" role 
                        await UserManager.AddToRoleAsync(administrator, "Admin");
                    }
                }
            }
            else
            {
                var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var administrator = new ApplicationUser
                {
                    UserName = configuration.GetSection("UserSettings")["UserEmail"],
                    Email = configuration.GetSection("UserSettings")["UserEmail"]
                };

                var userPassword = configuration.GetSection("UserSettings")["UserPassword"];
                var user = await UserManager.FindByEmailAsync(administrator.Email);

                if (user == null)
                {
                    var createPowerUser = await UserManager.CreateAsync(administrator, userPassword);
                    if (createPowerUser.Succeeded)
                    {
                        // here we assign the new user the "Admin" role 
                        await UserManager.AddToRoleAsync(administrator, "Admin");
                    }
                }
            }




            //var userPassword = configuration.GetSection("UserSettings")["UserPassword"];
            //var user = await UserManager.FindByEmailAsync(administrator.Email);
            
            //if (user == null)
            //{
            //    var createPowerUser = await UserManager.CreateAsync(administrator, userPassword);
            //    if (createPowerUser.Succeeded)
            //    {
            //        // here we assign the new user the "Admin" role 
            //        await UserManager.AddToRoleAsync(administrator, "Admin");
            //    }
            //}
        }
    }
}
