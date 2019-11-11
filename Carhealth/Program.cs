using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Carhealth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var host = new WebHostBuilder()
            //    .UseKestrel()  // настраиваем веб-сервер Kestrel 
            //    .UseContentRoot(Directory.GetCurrentDirectory())  // настраиваем корневой каталог приложения
            //    .UseIISIntegration() // обеспечиваем интеграцию с IIS
            //    .UseStartup<Startup>() // устанавливаем главный файл приложения
            //    .Build(); // создаем хост

            //host.Run(); // запускаем приложение



            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>();
    }
}
