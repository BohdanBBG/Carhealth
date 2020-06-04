using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Carhealth.Seed;
using CarHealth.Seed.Models;
using CarHealth.Seed.Repositories;
using CarHealth.Seed.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarHealth.Seed
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Title = "CarHealth.Seed";

            var configuration = BuildConfiguration();
            var serviceProvider = RegisterServices(configuration);

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

            // Override the current ILogger implementation to use Serilog
            //  services.AddLogging(configure => configure.AddSerilog());

            // TODO //ConfigureMongoDb(services, config);
            ConfigureEFCoreDb(services, config);

            services.AddTransient<IRepository<List<CarEntity>>, FileRepository>(xp =>
            { 
                return new FileRepository("AppData/data.json");
            });

            services.AddLogging();

            services.AddTransient<ISeedService, SeedServiceDevelopmentLocalhost>();

    // TODO //if (HostingEnvironmentHelper.IsDevelopmentLocalhost())
            //{
            //    services.AddTransient<ISeedService, SeedServiceDevelopmentLocalhost>();
            //}
            //else if (HostingEnvironmentHelper.IsDevelopmentHeroku())
            //{
            //    services.AddTransient<ISeedService, SeedServiceDevelopmentHeroku>();
            //}
            //else if (HostingEnvironmentHelper.IsProductionHeroku())
            //{
            //    services.AddTransient<ISeedService, SeedServiceProductionHeroku>();
            //}

            return services.BuildServiceProvider();
        }

        private static void ConfigureEFCoreDb(IServiceCollection services, ApplicationSettings config)
        {
            /// Before changing the repo type you should do:
            /// 1. Logout from app for clear cookie in your browser.
            /// 2. In Models\IdentityModels\Role.cs AND User.cs change parent class
            /// 3. Uncomit in Models\IdentityModels\UserContext.cs
            /// 4. Everywhere change Role to IdentityRole
            /// 5. Unhandled issue with UserContext and IdentityRole


            services.AddTransient<ICarRepository, EFCarRepository>(); // EF Core data repository

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
