using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

            var host = new WebHostBuilder()
                .UseKestrel()
                // задаём порт, и адрес на котором Kestrel будет слушать
                .UseUrls(new[] { "http://localhost:5003", "https://localhost:5004" })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
