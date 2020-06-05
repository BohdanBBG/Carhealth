using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CarHealth.Api.Helpers;
using CarHealth.Api.Models;
using CarHealth.Api.Models.IdentityModels;
using CarHealth.Api.Repositories;
using DotNetEnv;
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
        public static int Main(string[] args)
        {

            // check environment is set
            if (String.IsNullOrEmpty(HostingEnvironmentHelper.Environment))
            {
                Console.WriteLine("ASPNETCORE_ENVIRONMENT env variable must be set!");
                return 1;
            }



            Console.Title = "CarHealth.Api";

            var host = CreateWebHostBuilder(args).Build();

           // Console.WriteLine("-----" + HostingEnvironmentHelper.Environment);


            host.Run();

            return 0;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
              .UseConfiguration(GetConfiguration());
            //builder.UseUrls(new[] { "http://localhost:5000", "https://localhost:5001" });

            // builder.UseUrls($"https://localhost:5001");
            if (HostingEnvironmentHelper.IsDevelopmentLocalhost())
            {
                builder.UseUrls($"https://localhost:5001");
            }

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
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{HostingEnvironmentHelper.Environment}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            return config;
        }


      
    }
}
