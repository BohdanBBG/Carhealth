using AspNetCore.Identity.Mongo;
using CarHealth.Api;
using CarHealth.Api.Models.IdentityModels;
using CarHealth.Api.Repositories;
using CarHealth.ApiTest.Auth;
using CarHealth.ApiTest.TestRepositories;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;
using MongoDbGenericRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace CarHealth.ApiTest.Utils
{
    public class CustomWebApplicationBuilder<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public CustomWebApplicationBuilder() : base()
        {

        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            // Rewrite env variables for sure
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
            Environment.SetEnvironmentVariable("Environment", "Testing");

            var builder = WebHost.CreateDefaultBuilder()
                .UseEnvironment("Testing")
                .UseStartup<TStartup>();

            return builder;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // !!! Called after the Startup's service configuration and override it
            builder.ConfigureTestServices(services =>
            {
                var serviceProvider = services.BuildServiceProvider();
                ApplicationSettings config = serviceProvider.GetRequiredService<IOptions<ApplicationSettings>>().Value;
                IWebHostEnvironment hostingEnvironment = serviceProvider.GetService<IWebHostEnvironment>();

                ConfigureMongoDb(services, config);

               // use mocked auth
               JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = TestAuthenticationDefaults.AuthenticationScheme;
                }).AddTestAuthentication(options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = args =>
                        {
                            return Task.FromResult(0);
                        },
                    };
                });


            });
        }
        public void ConfigureMongoDb(IServiceCollection services, ApplicationSettings config)
        {

            services.AddSingleton<MongoDbRunner>(sp =>
            {
                return MongoDbRunner.Start();
            });

            services.AddTransient<MongoClient>(sp =>
             {
                 var mongoDbRunner = sp.GetRequiredService<MongoDbRunner>();

                 return new MongoClient(mongoDbRunner.ConnectionString);
             });

            services.AddTransient<ICarRepository, MongoRepository>(sp =>
            {
                var mongoClient = sp.GetRequiredService<MongoClient>();

                return new MongoRepository(mongoClient, config.MongoDb.MainDb);
            });

            services.AddTransient<IIdentityMongoRepository<User>, IdentityMongoRepository<User>>(sp =>
             {
                 var mongoClient = sp.GetRequiredService<MongoClient>();

                 return new IdentityMongoRepository<User>(mongoClient, config.MongoDb.MongoDbIdentity);
             });

        }
    }
}
