using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CarHealth.IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using CarHealth.IdentityServer4.Models.IdentityModels;
using IdentityServer4.Stores;
using CarHealth.IdentityServer4.Stores.EFCoreStores;
using CarHealth.IdentityServer4.Extensions;
using Microsoft.Extensions.Options;
using CarHealth.IdentityServer4.Contexts;
using MongoDB.Driver;
using AspNetCore.Identity.Mongo;

namespace CarHealth.IdentityServer4
{
    public class Startup
    {

        IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var config = Configuration.Get<ApplicationSettings>();

            services.Configure<ApplicationSettings>(Configuration);

           

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });




            var identityServerBuilder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })

            // добавляет тестовые ключи для подписи JWT-токенов, а именно id_token, access_token
            // В продакшне нужно заменить эти ключи, сделать это можно, например сгенерировав самоподписной сертификат:
            //https://brockallen.com/2015/06/01/makecert-and-creating-ssl-or-signing-certificates/
            .AddDeveloperSigningCredential();

             ConfigureMongoDb(services,identityServerBuilder, config);
            //ConfigureEFCoreDb((services,identityServerBuilder, config));

            identityServerBuilder.AddAspNetIdentity<User>();


            //.AddInMemoryPersistedGrants()

            // // что включать в id_token
            // //Почитать о том, что понимается под ресурсами можно http://docs.identityserver.io/en/release/topics/resources.html?highlight=identityresource
            // // а зачем они нужны — https://leastprivilege.com/2016/12/01/new-in-identityserver4-resource-based-configuration/
            // .AddInMemoryIdentityResources(GetIdentityResources())

            // // что включать в access_token
            // .AddInMemoryApiResources(GetApiResources())
            // // настройки клиентских приложений
            // .AddInMemoryClients(GetClients())
            // .AddAspNetIdentity<User>();

            //For IdentityServer UI check and install into project https://github.com/IdentityServer/IdentityServer4.Quickstart/releases/tag/1.5.0

            services.AddCors(options =>
            {
                if (config.Cors != null && config.Cors.AllowedOrigins != null)
                {
                    options.AddPolicy("default", builder =>
                    {
                        builder
                            .WithOrigins(config.Cors.AllowedOrigins.ToArray())
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true; // show detail of error and see the problem
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // app.UseHsts();
            }

            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseCors("default");

            app.UseHttpsRedirection();
            // app.UseCors("default");

            app.UseIdentityServer();

            app.UseMvcWithDefaultRoute();

        }

        private void ConfigureMongoDb(IServiceCollection services, IIdentityServerBuilder identityServerBuilder, ApplicationSettings config)
        {
            services.AddTransient<MongoClient>(sp =>
            {
                return new MongoClient(config.MongoDb.ConnectionString);
            });

            identityServerBuilder.AddMongoClients(config)
                                 .AddMongoIdentityApiResources(config)
                                 .AddMongoDbForAspIdentity<User, Role>(config);
        }

        private void ConfigureEFCoreDb(IServiceCollection services, IIdentityServerBuilder identityServerBuilder, ApplicationSettings config)
        {
           // services.adddbcontext<usercontext>(options =>
           //    options.usesqlserver(config.efcoredb.usersidentitydb)
           //);

           // services.adddbcontext<identityservercontext>(options =>
           //  options.usesqlserver(config.efcoredb.clientsidentitydb)); // repository for identityserver (clients, scopes, etc)

           // identityServerBuilder.AddEFCoreClients()
           //                     .AddEFCoreIdentityApiResources();

           // services.addidentity<user, role>(options => //валидация пароля 
           // {
           //     options.password.requiredlength = 4;   // минимальная длина
           //     options.password.requirenonalphanumeric = false;   // требуются ли не алфавитно-цифровые символы
           //     options.password.requirelowercase = false; // требуются ли символы в нижнем регистре
           //     options.password.requireuppercase = false; // требуются ли символы в верхнем регистре
           //     options.password.requiredigit = false; // требуются ли цифры
           //     options.user.requireuniqueemail = true; // уникальный email
           // })
           // .addentityframeworkstores<usercontext>()
           // .adddefaulttokenproviders();// устанавливает тип хранилища, которое будет применяться в identity для хранения 
           //                             // данных.в качестве типа хранилища здесь указывается класс контекста данных.
        }
    }
}
