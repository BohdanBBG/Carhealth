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

            //  services.AddLogging();


            ConfigureMongoDb(services, config);
            //ConfigureEFCoreDb(services, config);


              services.AddTransient<IClientStore, EFCoreClientStore>();
              services.AddTransient<IResourceStore, EFCoreResourceStore>();

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
            .AddDeveloperSigningCredential()
            .AddClients()
            .AddIdentityApiResources()
            .AddAspNetIdentity<User>();

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
                options.AddPolicy("default", builder =>
                {
                    builder
                        .WithOrigins(config.Cors.AllowedOrigins.ToArray())
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
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

        private void ConfigureMongoDb(IServiceCollection services, ApplicationSettings config)
        {
            services.AddTransient<MongoClient>(sp =>
            {
                return new MongoClient(config.MongoDb.ConnectionString);
            });


            services.AddIdentityMongoDbProvider<User, Role>(identityOptions =>
            {
                identityOptions.Password.RequiredLength = 4;   // минимальная длина
                identityOptions.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                identityOptions.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                identityOptions.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                identityOptions.Password.RequireDigit = false; // требуются ли цифры
                identityOptions.User.RequireUniqueEmail = true; // уникальный email
            }, mongoIdentityOptions =>
            {
                mongoIdentityOptions.ConnectionString = config.MongoDb.MongoDbIdentity;
            });
        }

        private void ConfigureEFCoreDb(IServiceCollection services, ApplicationSettings config)
        {
            // services.AddDbContext<UserContext>(options =>
            //    options.UseSqlServer(config.EFCoreDb.UsersIdentityDb)
            //);

            services.AddDbContext<IdentityServerContext>(options =>
             options.UseSqlServer(config.EFCoreDb.ClientsIdentityDb)); // repository for IdentityServer (clients, scopes, etc)

            //services.AddIdentity<User, Role>(options => //валидация пароля 
            //{
            //    options.Password.RequiredLength = 4;   // минимальная длина
            //    options.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
            //    options.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
            //    options.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
            //    options.Password.RequireDigit = false; // требуются ли цифры
            //    options.User.RequireUniqueEmail = true; // уникальный email
            //})
            //.AddEntityFrameworkStores<UserContext>()
            //.AddDefaultTokenProviders();// устанавливает тип хранилища, которое будет применяться в Identity для хранения 
            //                            // данных.В качестве типа хранилища здесь указывается класс контекста данных.
        }

        //public static IEnumerable<IdentityResource> GetIdentityResources()//Настройки информации для клиентских приложений
        //{
        //    // определяет, какие scopes будут доступны IdentityServer
        //    return new List<IdentityResource>
        //    {
        //        // "sub" claim
        //        new IdentityResources.OpenId(),
        //       new IdentityResources.Email(), // profile Claims: email and email_verified
        //         // стандартные claims в соответствии с profile scope
        //         // http://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims
        //       new IdentityResources.Profile(),//  profile Claims: name, family_name, given_name, middle_name, nickname etc
        //    };

        //}

        //public static IEnumerable<ApiResource> GetApiResources()// информация предназначается для API.
        //                                                        //Чтобы разрешить клиентам запрашивать токены доступа для API, необходимо определить ресурсы API
        //{
        //    // claims этих scopes будут включены в access_token
        //    return new List<ApiResource>
        //    {
        //        // определяем scope "CarHealth.Api" для IdentityServer
        //        new ApiResource("CarHealth.Api","Carhealth Api",
        //        // эти claims войдут в scope CarHealth.Api
        //        new[] { "UserName", "email"})

        //    };
        //}

        //public static IEnumerable<Client> GetClients()//Сами клиентские приложения, нужно чтобы сервер знал о них
        //{
        //    return new List<Client>
        //    {
        //        new Client
        //        { 
        //            // обязательный параметр, при помощи client_id сервер различает клиентские приложения 
        //            ClientId = "CarHealth.Web",
        //            ClientName = "Web",
        //            AllowedGrantTypes = GrantTypes.Implicit,
        //            AllowAccessTokensViaBrowser = true,
        //             // от этой настройки зависит размер токена, 
        //             // при false можно получить недостающую информацию через UserInfo endpoint
        //            AlwaysIncludeUserClaimsInIdToken = true,
        //            // белый список адресов на который клиентское приложение может попросить
        //            // перенаправить User Agent, важно для безопасности
        //            RedirectUris =
        //            {
        //                // адрес перенаправления после логина
        //                "https://localhost:5004/callback.html",
        //                // адрес перенаправления при автоматическом обновлении access_token через iframe
        //                 "https://localhost:5004/callback-silent.html"
        //            },
        //            PostLogoutRedirectUris= { "https://localhost:5004/index.html" },
        //            // адрес клиентского приложения, просим сервер возвращать нужные CORS-заголовки
        //            AllowedCorsOrigins = { "https://localhost:5004" },
        //             // список scopes, разрешённых именно для данного клиентского приложения
        //            AllowedScopes =
        //            {
        //                IdentityServerConstants.StandardScopes.OpenId,
        //                IdentityServerConstants.StandardScopes.Profile,
        //                 IdentityServerConstants.StandardScopes.Email,
        //                "CarHealth.Api"
        //            },
        //            AccessTokenLifetime = 3600,// секунд, это значение по умолчанию
        //            IdentityTokenLifetime = 300, // секунд, это значение по умолчанию

        //             // разрешено ли получение refresh-токенов через указание scope offline_access
        //             AllowOfflineAccess = false,
        //             RequireConsent = false
        //        }
        //    };
        //}

    }
}
