﻿using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
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
                .UseStartup<Startup>()
                .UseSerilog((context, configuration) =>
            {
                configuration
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .WriteTo.File(@"serilog-logs/identityserver4_log.txt")
                    // .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
                    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss}|{Level} RequestPath:{RequestPath} => {SourceContext}{NewLine} {Message}{NewLine}{Exception}", theme: AnsiConsoleTheme.Literate);
            });

            return builder;
        }
    }
}
