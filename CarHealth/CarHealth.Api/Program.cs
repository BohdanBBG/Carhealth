using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CarHealth.Api.Models;
using CarHealth.Api.Models.IdentityModels;
using CarHealth.Api.Repositories;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CarHealth.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
          

            Console.Title = "CarHealth.Api";

            var host = CreateWebHostBuilder(args).Build();

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
              .UseConfiguration(GetConfiguration())
              .UseUrls(new[] { "http://localhost:5000", "https://localhost:5001" });


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
