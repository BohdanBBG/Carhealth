using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CarHealth.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "CarHealth.Web";

            CreateWebHostBuilder(args).Build().Run();

            //var host = new WebHostBuilder()
            //    .UseKestrel()
            //    // задаём порт, и адрес на котором Kestrel будет слушать
            //    .UseUrls("https://localhost:5004" )
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseIISIntegration()
            //    .UseStartup<Startup>()
            //    .Build();

            //host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configBuilder) =>
                {
                    // load env variables from .env file
                    string envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
                    if (File.Exists(envFilePath))
                    {
                        DotNetEnv.Env.Load(envFilePath);
                    }

                    configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    configBuilder.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    configBuilder.AddEnvironmentVariables();
                });

            if (System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "DevelopmentLocalhost")
            {
                builder.UseUrls($"http://localhost:5003");
            }

            builder.UseStartup<Startup>();

            return builder;
        }
    }
}
