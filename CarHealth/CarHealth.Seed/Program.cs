using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Carhealth.Seed;
using CarHealth.Seed.Models;
using CarHealth.Seed.Repositories;
using CarHealth.Seed.SeedServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CarHealth.Seed
{
    public class Program
    {
        public static string Environment
        {
            get
            {
                // for web projects
                string env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                // for console projects
                if (String.IsNullOrEmpty(env))
                {
                    env = System.Environment.GetEnvironmentVariable("Environment");
                }

                if (String.IsNullOrEmpty(env))
                {
                    // if we get here then Environment is possibly set in hostsettings.json and can't be accessed
                    // only using 'hostingContext.HostingEnvironment.EnvironmentName', so it is not in
                    // 'Environment' env variable
                    throw new Exception("Neither ASPNETCORE_ENVIRONMENT not Environment is set! Recheck startup configuration.");
                }

                return env;
            }
        }

        public static async Task Main(string[] args)
        {
            Console.Title = "CarHealth.Seed";

            var configuration = BuildConfiguration();
            var serviceProvider = RegisterServices(configuration);
            var logger = serviceProvider.GetService<ILogger<Program>>();

            logger.LogInformation(" \n");
            logger.LogInformation("Parameters:");
            logger.LogInformation("Environment: {Environment}", System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            logger.LogInformation("\n");

            var seedService = serviceProvider.GetService<ISeedService>();

            //seed
            await seedService.SeedAsync();

        }
       
        private static IConfigurationRoot BuildConfiguration()
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

            return builder.Build();
        }

        private static IServiceProvider RegisterServices(IConfiguration configuration)
        {
            var config = configuration.Get<ApplicationSettings>();
            var services = new ServiceCollection();

            services.AddOptions();
            services.Configure<ApplicationSettings>(configuration);
            //services.Configure<Lexiconner.IdentityServer4.ApplicationSettings>(configuration); // map current config to identity  config

            //register directly to access using DI
            services.AddTransient(sp => configuration);

            services.AddLogging(configure => configure.AddConsole())
                .AddTransient<Program>();

            // Override the current ILogger implementation to use Serilog

            // TODO //ConfigureMongoDb(services, config);
            ConfigureEFCoreDb(services, config);

            services.AddTransient<IMainDbSeed<List<CarEntity>>, FileRepository>(xp =>
            {
                return new FileRepository("AppData/data.json");
            });


            if (Environment == "DevelopmentLocalhost")
            {
                services.AddTransient<ISeedService, SeedServiceDevelopmentLocalhost>();
            }


            return services.BuildServiceProvider();
        }


        private static void ConfigureEFCoreDb(IServiceCollection services, ApplicationSettings config)
        {

            services.AddTransient<ICarRepository, EFMainDbSeed>(); // EF Core data repository

            services.AddDbContext<CarContext>(options =>
           options.UseSqlServer(config.EFCoreDb.CarsDb)); // for EF Core data repository

            services.AddDbContext<UserContext>(options =>
           options.UseSqlServer(config.EFCoreDb.CarHealthIdentityDb));

            services.AddIdentity<User, IdentityRole>(options => //валидация пароля 
            {
                options.Password.RequiredLength = 4;   // минимальная длина
                options.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                options.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                options.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                options.Password.RequireDigit = false; // требуются ли цифры
                options.User.RequireUniqueEmail = true; // уникальный email
            })
            .AddEntityFrameworkStores<UserContext>();// устанавливает тип хранилища, которое будет применяться в Identity для хранения 
                                                     // данных.В качестве типа хранилища здесь указывается класс контекста данных.

        }

        private static void ConfigureMongoDb(IServiceCollection services, ApplicationSettings config)
        {

            //services.AddTransient<MongoClient>(sp =>
            //{
            //    return new MongoClient(config.MongoDb.ConnectionString);
            //});


            //services.AddTransient<ICarRepository, MongoCarsRepository>(sp =>
            //{
            //    var mongoClient = sp.GetService<MongoClient>();

            //    return new MongoCarsRepository(mongoClient, config.MongoDb.MainDb);
            //}); // MongoDb data repository


            //services.AddIdentityMongoDbProvider<User, Role>(identityOptions =>
            //{
            //    identityOptions.Password.RequiredLength = 4;   // минимальная длина
            //    identityOptions.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
            //    identityOptions.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
            //    identityOptions.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
            //    identityOptions.Password.RequireDigit = false; // требуются ли цифры
            //    identityOptions.User.RequireUniqueEmail = true; // уникальный email
            //}, mongoIdentityOptions =>
            //{
            //    mongoIdentityOptions.ConnectionString = config.MongoDb.MongoDbIdentity;
            // });
        }

    }
}
