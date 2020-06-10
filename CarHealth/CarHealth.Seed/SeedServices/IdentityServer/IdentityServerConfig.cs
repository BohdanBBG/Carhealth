using Carhealth.Seed;
using CarHealth.Seed.Models;
using CarHealth.Seed.Models.IdentityModels;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.SeedServices.IdentityServer
{
    public class IdentityServerConfig : IIdentityServerConfig
    {

        public string DefaultUserPassword { get { return "1234"; } }

        public IEnumerable<ApiResource> GetApiResources()
        {
            // claims этих scopes будут включены в access_token
            return new List<ApiResource>
            {
                // определяем scope "CarHealth.Api" для IdentityServer
                new ApiResource("CarHealth.Api","Carhealth Api",
                // эти claims войдут в scope CarHealth.Api
                new[] { "UserName", "email"}),

            };
        }

        public IEnumerable<Client> GetClients(ApplicationSettings config)
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
                        $"{config.Urls.WebSpa}",
                        $"{config.Urls.WebSpa}/index.html",
                        // адрес перенаправления после логина
                        $"{config.Urls.WebSpa}/callback.html",
                          // адрес перенаправления при автоматическом обновлении access_token через iframe
                         $"{config.Urls.WebSpa}/callback-silent.html"
                    },
                    PostLogoutRedirectUris= {    $"{config.Urls.WebSpa}/index.html" },
                    // адрес клиентского приложения, просим сервер возвращать нужные CORS-заголовки
                    AllowedCorsOrigins = {  $"{config.Urls.WebSpa}" },
                     // список scopes, разрешённых именно для данного клиентского приложения
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                         IdentityServerConstants.StandardScopes.Email,
                        "CarHealth.Api"
                    },
                    AccessTokenLifetime = 3600,// секунд, это значение по умолчанию
                    IdentityTokenLifetime = 300, // секунд, это значение по умолчанию

                     // разрешено ли получение refresh-токенов через указание scope offline_access
                     AllowOfflineAccess = false,
                     RequireConsent = false
                }
            };
        }

        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            // определяет, какие scopes будут доступны IdentityServer
            return new List<IdentityResource>
            {
                // "sub" claim
                new IdentityResources.OpenId(),
               new IdentityResources.Email(), // profile Claims: email and email_verified
                 // стандартные claims в соответствии с profile scope
                 // http://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims
               new IdentityResources.Profile(),//  profile Claims: name, family_name, given_name, middle_name, nickname etc
            };
        }

        public List<User> GetInitialdentityUsers()
        {
            return new List<User>
            {
               new User
                {
                    Email = "admin1@gmail.com",
                    UserName = "admin1@gmail.com"
                },
                new User
                {
                     Email = "user1@gmail.com",
                     UserName ="user1@gmail.com"
                }
            };
        }

        public List<IdentityRole> GetInitialIdentityRoles()
        {
            return new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin"
                },
                 new IdentityRole
                {
                    Name = "User"
                }
            };
        }
    }
}
