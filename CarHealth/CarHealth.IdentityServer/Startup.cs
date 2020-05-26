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

namespace CarHealth.IdentityServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddIdentityServer(options =>
            {
                options.Endpoints = new EndpointsOptions
                {
                    // в Implicit Flow используется для получения токенов
                    EnableAuthorizeEndpoint = true,
                    // для получения статуса сессии
                    EnableCheckSessionEndpoint = true,
                    // для логаута по инициативе пользователя
                    EnableEndSessionEndpoint = true,
                    // для получения claims аутентифицированного пользователя 
                    // http://openid.net/specs/openid-connect-core-1_0.html#UserInfo
                    EnableUserInfoEndpoint = true,
                    // используется OpenId Connect для получения метаданных
                    EnableDiscoveryEndpoint = true,

                    // для получения информации о токенах, мы не используем
                    EnableIntrospectionEndpoint = false,
                    // нам не нужен т.к. в Implicit Flow access_token получают через authorization_endpoint
                    EnableTokenEndpoint = false,
                    // мы не используем refresh и reference tokens 
                    // http://docs.identityserver.io/en/release/topics/reference_tokens.html
                    EnableTokenRevocationEndpoint = false
                };

                // IdentitySever использует cookie для хранения своей сессии
                options.Authentication = new IdentityServer4.Configuration.AuthenticationOptions
                {
                    CookieLifetime = TimeSpan.FromDays(1)
                };
            })
                // добавляет тестовые ключи для подписи JWT-токенов, а именно id_token, access_token
                // В продакшне нужно заменить эти ключи, сделать это можно, например сгенерировав самоподписной сертификат:
                //https://brockallen.com/2015/06/01/makecert-and-creating-ssl-or-signing-certificates/
                .AddDeveloperSigningCredential()

                // что включать в id_token
                //Почитать о том, что понимается под ресурсами можно http://docs.identityserver.io/en/release/topics/resources.html?highlight=identityresource
                // а зачем они нужны — https://leastprivilege.com/2016/12/01/new-in-identityserver4-resource-based-configuration/
                .AddInMemoryIdentityResources(GetIdentityResources())

                // что включать в access_token
                .AddInMemoryApiResources(GetApiResources())
                // настройки клиентских приложений
                .AddInMemoryClients(GetClients())
                // тестовые пользователи
                .AddTestUsers(GetUsers());


            //For IdentityServer UI check and install into project https://github.com/IdentityServer/IdentityServer4.Quickstart.UI/releases/tag/1.5.0
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDeveloperExceptionPage();

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()//Настройки информации для клиентских приложений
        {
            // определяет, какие scopes будут доступны IdentityServer
            return new List<IdentityResource>
            {
                // "sub" claim
                new IdentityResources.OpenId(),
              // new IdentityResources.Email(), // profile Claims: email and email_verified
                 // стандартные claims в соответствии с profile scope
                 // http://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims
               new IdentityResources.Profile(),//  profile Claims: name, family_name, given_name, middle_name, nickname etc
            };

        }

        public static IEnumerable<ApiResource> GetApiResources()// информация предназначается для API.
                                                                //Чтобы разрешить клиентам запрашивать токены доступа для API, необходимо определить ресурсы API
        {
            // claims этих scopes будут включены в access_token
            return new List<ApiResource>
            {
                // определяем scope "CarHealth.Api" для IdentityServer
                new ApiResource("CarHealth.Api","Carhealth Api",
                // эти claims войдут в scope api1
                new[] { "name", "role"})

            };
        }

        public static IEnumerable<Client> GetClients()//Сами клиентские приложения, нужно чтобы сервер знал о них
        {
            return new List<Client>
            {
                new Client
                { 
                    // обязательный параметр, при помощи client_id сервер различает клиентские приложения 
                    ClientId = "CarHealth.Web",
                    ClientName = "Web",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                     // от этой настройки зависит размер токена, 
                     // при false можно получить недостающую информацию через UserInfo endpoint
                    AlwaysIncludeUserClaimsInIdToken = true,
                    // белый список адресов на который клиентское приложение может попросить
                    // перенаправить User Agent, важно для безопасности
                    RedirectUris =
                    {
                        // адрес перенаправления после логина
                        "http://localhost:5003/callback.html",
                        // адрес перенаправления при автоматическом обновлении access_token через iframe
                         "http://localhost:5003/callback-silent.html"
                    },
                    PostLogoutRedirectUris= { "http://localhost:5003/index.html" },
                    // адрес клиентского приложения, просим сервер возвращать нужные CORS-заголовки
                    AllowedCorsOrigins = { "http://localhost:5003" },
                     // список scopes, разрешённых именно для данного клиентского приложения
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "CarHealth.Api"
                    },
                    AccessTokenLifetime = 3600,// секунд, это значение по умолчанию
                    IdentityTokenLifetime = 300, // секунд, это значение по умолчанию

                     // разрешено ли получение refresh-токенов через указание scope offline_access
                     AllowOfflineAccess = false,
                }
            };
        }

        public static List<TestUser> GetUsers() //Тестовые пользователи, bob админ
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "1234",

                    Claims = new List<Claim>
                    {
                        new Claim("name","Alice"),
                        new Claim("email","user@email.com"),
                        new Claim("website","https://alice.com"),
                        new Claim("role","user")
                    }
                },
                new TestUser
                {
                     SubjectId = "2",
                     Username = "bob",
                     Password = "password",

                     Claims = new List<Claim>
                     {
                        new Claim("name", "Bob"),
                        new Claim("email","admin@email.com"),
                        new Claim("website", "https://bob.com"),
                        new Claim("role", "admin"),
                     }
                }
            };
        }
    }
}
