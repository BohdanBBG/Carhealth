using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CarHealth.IdentityServer4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "IdentityServer";

            var host = new WebHostBuilder()
                .UseKestrel()
                // задаём порт, и адрес на котором Kestrel будет слушать
                .UseUrls(new[] { "http://localhost:5005", "https://localhost:5006" })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

    }
}
