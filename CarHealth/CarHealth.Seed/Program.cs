using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CarHealth.Seed.Models;
using CarHealth.Seed.Models.IdentityModels;
using CarHealth.Seed.Repositories;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CarHealth.Seed
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Title = "CarHealth.Seed";

            Console.WriteLine("Data seed");
            //Console.WriteLine("Data seed application: Started");

            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {

                    var userManager = services.GetRequiredService<UserManager<User>>();

                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    var fileRepository = services.GetRequiredService< IRepository<List<CarEntity>>>();
                    var carRepository = services.GetRequiredService<ICarRepository>();

                    await RoleInitializer.InitializeAsync(userManager, roleManager);

                    if (carRepository.IsEmptyDb())
                    {
                        await CarsDbInitializer.InitializeAsync(fileRepository, userManager, carRepository);
                    }
                }

                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
             .UseConfiguration(GetConfiguration())
             .UseUrls(new[] { "http://localhost:5007", "https://localhost:5008" });

            builder.UseStartup<Startup>();

            return builder;
        }

        private static IConfiguration GetConfiguration()
        {
            // load env variables from .env file
            string envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

            if (File.Exists(envFilePath))
            {
                DotNetEnv.Env.Load(envFilePath);
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            return config;
        }

    }
}
