using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AspNetCore.Identity.Mongo;
using Carhealth.Seed;
using CarHealth.Seed.Contexts;
using CarHealth.Seed.Models;
using CarHealth.Seed.Models.IdentityModels;
using CarHealth.Seed.Repositories;
using CarHealth.Seed.Repositories.EFCoreDb;
using CarHealth.Seed.Repositories.MongoDb;
using CarHealth.Seed.SeedServices;
using CarHealth.Seed.SeedServices.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CarHealth.Seed
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Title = "CarHealth.Seed";

            var configuration = BuildConfiguration();
            var serviceProvider = RegisterServices(configuration);
            var logger = serviceProvider.GetService<ILogger<Program>>();


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

            Console.WriteLine("ASPNETCORE_ENVIRONMENT: " + System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        private static IServiceProvider RegisterServices(IConfiguration configuration)
        {
            var config = configuration.Get<ApplicationSettings>();
            var services = new ServiceCollection();

            services.AddOptions();
            services.Configure<ApplicationSettings>(configuration);

            //register directly to access using DI
            services.AddTransient(sp => configuration);

            services.AddLogging(configure => configure.AddConsole())
                .AddTransient<Program>();

            services.AddTransient<IIdentityServerConfig, IdentityServerConfig>();


            services.AddTransient<IDbFileReader<List<CarEntity>>, DbFileReader>(xp =>
            {
                return new DbFileReader(config.Import.FilePath);
            });

            ConfigureMongoDb(services, config);
            //ConfigureEFCoreDb(services, config);


            services.AddTransient<ISeedService, SeedServiceDevelopmentLocalhost>();


            return services.BuildServiceProvider();
        }


        private static void ConfigureEFCoreDb(IServiceCollection services, ApplicationSettings config)
        {

           // services.AddTransient<ISeedRepository, EFMainDbSeedRepository>(); // EF Core data repository

           // services.AddTransient<IIdentitySeedRepository, EFCoreIdentitySeedRepository>();// // EF Core identity data repository

           // services.AddDbContext<CarContext>(options =>
           //options.UseSqlServer(config.EFCoreDb.CarsDb)); // for EF Core data repository

           // services.AddDbContext<IdentityServerContext>(options =>
           //options.UseSqlServer(config.EFCoreDb.ClientsIdentityDb)); // repository for IdentityServer (clients, scopes, etc)

           // services.AddDbContext<UserContext>(options =>
           //options.UseSqlServer(config.EFCoreDb.UsersIdentityDb));

           // services.AddIdentity<User, Role>(options => //валидация пароля 
           // {
           //     options.Password.RequiredLength = 4;   // минимальная длина
           //     options.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
           //     options.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
           //     options.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
           //     options.Password.RequireDigit = false; // требуются ли цифры
           //     options.User.RequireUniqueEmail = true; // уникальный email
           // })
           // .AddEntityFrameworkStores<UserContext>();// устанавливает тип хранилища, которое будет применяться в Identity для хранения 
           //                                          // данных.В качестве типа хранилища здесь указывается класс контекста данных.

        }

        private static void ConfigureMongoDb(IServiceCollection services, ApplicationSettings config)
        {

            services.AddTransient<MongoClient>(sp =>
            {
                return new MongoClient(config.MongoDb.ConnectionString);
            });

            services.AddTransient<IIdentitySeedRepository, MongoIdentitySeedRepository>(sp =>
            {
                var mongoClient = sp.GetService<MongoClient>();
                var userManager = sp.GetService<UserManager<User>>();
                var roleManager = sp.GetService<RoleManager<Role>>();

                return new MongoIdentitySeedRepository(mongoClient, config.MongoDb.MongoDbIdentity, userManager, roleManager);
            });// Mongo identity data repository


            services.AddTransient<ISeedRepository, MongoMainDbSeedRepository>(sp =>
            {
                var mongoClient = sp.GetService<MongoClient>();

                return new MongoMainDbSeedRepository(mongoClient, config.MongoDb.MainDb);
            }); // MongoDb data repository

            services.AddIdentity<User, Role>()
            .AddMongoDbStores<User, Role, string>(config.MongoDb.ConnectionString, config.MongoDb.MongoDbIdentity)
              .AddDefaultTokenProviders();

        }

    }
}
