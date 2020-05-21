﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Carhealth.Models;
using Carhealth.Models.IdentityModels;
using Carhealth.Repositories;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Carhealth
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //var host = new WebHostBuilder()
            //    .UseKestrel()  // настраиваем веб-сервер Kestrel 
            //    .UseContentRoot(Directory.GetCurrentDirectory())  // настраиваем корневой каталог приложения
            //    .UseIISIntegration() // обеспечиваем интеграцию с IIS
            //    .UseStartup<Startup>() // устанавливаем главный файл приложения
            //    .Build(); // создаем хост

            //host.Run(); // запускаем приложение

          

            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {

                    var userManager = services.GetRequiredService<UserManager<User>>(); 

                    var roleManager = services.GetRequiredService<RoleManager<Role>>(); 

                    var fileRepository = services.GetRequiredService<IRepository<List<CarEntity>>>();
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
              .UseConfiguration(GetConfiguration());

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