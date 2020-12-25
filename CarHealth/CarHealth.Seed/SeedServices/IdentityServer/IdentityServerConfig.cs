using Carhealth.Seed;
using CarHealth.Seed.Models;
using CarHealth.Seed.Models.IdentityModels;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.SeedServices.IdentityServer
{
    public class IdentityServerConfig : IIdentityServerConfig
    {

        public string DefaultUserPassword { get { return "Password_1"; } }

        public IEnumerable<ApiResource> GetApiResources()
        {
            // claims этих scopes будут включены в access_token
            return new List<ApiResource>
            {
                new ApiResource("CarHealth.Api","Carhealth Api",
                new List<string>{})
            };
        }

        public IEnumerable<Client> GetClients(IConfiguration config)
        {
            return new List<Client>
            {
                new Client
                { 
                    ClientId = "CarHealth.Web",
                    ClientName = "Web",
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris =
                    {
                         $"{config.Get<ApplicationSettings>().Urls.WebSpa}/callback.html",
                         $"{config.Get<ApplicationSettings>().Urls.WebSpa}/callback-silent.html",

                    },
                    PostLogoutRedirectUris=
                    {
                      $"{config.Get<ApplicationSettings>().Urls.WebSpa}/index.html",
                      $"{config.Get<ApplicationSettings>().Urls.WebSpa}",

                    },
                    AllowedCorsOrigins =
                    {
                        $"{config.Get<ApplicationSettings>().Urls.WebSpa}",
                    },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                         IdentityServerConstants.StandardScopes.Email,
                        "CarHealth.Api"
                    },
                    AccessTokenLifetime = 3600,// секунд, это значение по умолчанию
                    IdentityTokenLifetime = 300, // секунд, это значение по умолчанию
                    RequireClientSecret = false,

                     // разрешено ли получение refresh-токенов через указание scope offline_access
                     AllowOfflineAccess = false,
                     RequireConsent = false
                },
                 new Client
                {
                    ClientId = "CarHealth.Web.React",
                    ClientName = "Web.React",
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris =
                    {
                         $"{config.Get<ApplicationSettings>().Urls.WebSpaReact}/callback.html",
                         $"{config.Get<ApplicationSettings>().Urls.WebSpaReact}/callback-silent.html"

                    },
                    PostLogoutRedirectUris=
                    {
                         $"{config.Get<ApplicationSettings>().Urls.WebSpaReact}",
                         $"{config.Get<ApplicationSettings>().Urls.WebSpaReact}/index.html"
                    },
                    AllowedCorsOrigins =
                    {
                        $"{config.Get<ApplicationSettings>().Urls.WebSpa}",
                        $"{config.Get<ApplicationSettings>().Urls.WebSpaReact}"
                    },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                         IdentityServerConstants.StandardScopes.Email,
                        "CarHealth.Api"
                    },
                    AccessTokenLifetime = 3600,// секунд, это значение по умолчанию
                    IdentityTokenLifetime = 300, // секунд, это значение по умолчанию
                    RequireClientSecret = false,

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
                   // Id = ObjectId.GenerateNewId().ToString(),
                    Email = "admin1@gmail.com",
                    UserName = "admin1@gmail.com"
                },
                new User
                {
                   //  Id = ObjectId.GenerateNewId().ToString(),
                     Email = "user1@gmail.com",
                     UserName ="user1@gmail.com"
                }
            };
        }

        public List<Role> GetInitialIdentityRoles()
        {
            return new List<Role>
            {
                new Role
                {
                    //Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Admin"
                },
                 new Role
                {
                   // Id = ObjectId.GenerateNewId().ToString(),
                    Name = "User"
                }
            };
        }
    }
}
