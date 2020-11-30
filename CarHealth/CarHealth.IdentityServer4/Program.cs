using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CarHealth.IdentityServer4.Models;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace CarHealth.IdentityServer4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "IdentityServer";

            //var host = new WebHostBuilder()
            //    .UseKestrel()
            //    // задаём порт, и адрес на котором Kestrel будет слушать
            //    .UseUrls(new[] { "http://localhost:5005", "https://localhost:5006" })
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseIISIntegration()
            //    .UseStartup<Startup>()
            //    .Build();

            //host.Run();
            var host = CreateWebHostBuilder(args).Build();

            host.Run();

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(GetConfiguration())
                .UseStartup<Startup>()
                .UseSerilog((context, configuration) =>
            {
                configuration
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                   // .WriteTo.File(@"serilog-logs/identityserver4_log.txt")
                     .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss}|{Level} RequestPath:{RequestPath} => {SourceContext}{NewLine} {Message}{NewLine}{Exception}", theme: AnsiConsoleTheme.Literate);
            });


            if (System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "DevelopmentLocalhost")
            {
                builder.UseUrls($"http://localhost:5005");
            }

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
                .AddJsonFile($"appsettings.{System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            return config;
        }

    }
}
