using AspNetCore.Identity.Mongo;
using CarHealth.IdentityServer4.Models.IdentityModels;
using CarHealth.IdentityServer4.Stores;
using CarHealth.IdentityServer4.Stores.EFCoreStores;
using CarHealth.IdentityServer4.Stores.MongoDbStores;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.IdentityServer4.Extensions
{
    public static class IdentityServerBuilderExtensions
    {

        static public IIdentityServerBuilder AddEFCoreClients(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IClientStore, EFCoreClientStore>();

            return builder;
        }

        static public IIdentityServerBuilder AddEFCoreIdentityApiResources(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IResourceStore, EFCoreResourceStore>();

            return builder;
        }

        static public IIdentityServerBuilder AddMongoClients(this IIdentityServerBuilder builder, ApplicationSettings config)
        {
            builder.Services.AddTransient<IClientStore, MongoDbClientStore>(sp =>
            {
                var mongoClient = sp.GetRequiredService<MongoClient>();

                return new MongoDbClientStore(mongoClient, config.MongoDb.MongoDbIdentity); 
            });

            return builder;
        }

        static public IIdentityServerBuilder AddMongoIdentityApiResources(this IIdentityServerBuilder builder, ApplicationSettings config)
        {
            builder.Services.AddTransient<IResourceStore, MongoDbResourceStore>(sp =>
            {
                var mongoClient = sp.GetRequiredService<MongoClient>();

                return new MongoDbResourceStore(mongoClient, config.MongoDb.MongoDbIdentity);
            });
            return builder;
        }

        static public IIdentityServerBuilder AddMongoDbForAspIdentity<TIdentity, TRole> (this IIdentityServerBuilder builder, ApplicationSettings config)
            where TIdentity : User, new()
            where TRole : Role, new()
        {

            builder.Services.AddIdentity<TIdentity, TRole>()
              .AddMongoDbStores<TIdentity, TRole, string>(config.MongoDb.ConnectionString, config.MongoDb.MongoDbIdentity)
              .AddDefaultTokenProviders();

            return builder;
        }
    }
}
