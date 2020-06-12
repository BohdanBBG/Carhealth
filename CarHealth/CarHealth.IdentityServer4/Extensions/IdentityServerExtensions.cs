using CarHealth.IdentityServer4.Stores;
using CarHealth.IdentityServer4.Stores.EFCoreStores;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.IdentityServer4.Extensions
{
    public static class IdentityServerExtensions
    {

        static public IIdentityServerBuilder AddClients(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IClientStore, EFCoreClientStore>();

            return builder;
        }

        static public IIdentityServerBuilder AddIdentityApiResources(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IResourceStore, EFCoreResourceStore>();

            return builder;
        }
    }
}
