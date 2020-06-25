using CarHealth.IdentityServer4.Models.IdentityModels;
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
    public static class IdentityServerBuilderExtensions
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

        static public IIdentityServerBuilder AddMongoDbForAspIdentity<TIdentity, TRole> (this IIdentityServerBuilder builder, ApplicationSettings config)
            where TIdentity : User, new()
            where TRole : Role, new()
        {
            //services.AddTransient<MongoClient>(sp =>
            //{
            //    return new MongoClient(config.MongoDb.ConnectionString);
            //});

            //services.AddTransient<IIdentitySeedRepository, MongoIdentitySeedRepository>(sp =>
            //{
            //    var mongoClient = sp.GetService<MongoClient>();
            //    var userManager = sp.GetService<UserManager<User>>();
            //    var roleManager = sp.GetService<RoleManager<Role>>();

            //    return new MongoIdentitySeedRepository(mongoClient, config.MongoDb.MongoDbIdentity, userManager, roleManager);
            //});// Mongo identity data repository



            //services.AddTransient<ISeedRepository, MongoMainDbSeedRepository>(sp =>
            //{
            //    var mongoClient = sp.GetService<MongoClient>();

            //    return new MongoMainDbSeedRepository(mongoClient, config.MongoDb.MainDb);
            //}); // MongoDb data repository



            //services.AddIdentityMongoDbProvider<User, Role>(op =>
            //{
            //    op.Password.RequiredLength = 4;   // минимальная длина
            //    op.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
            //    op.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
            //    op.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
            //    op.Password.RequireDigit = false; // требуются ли цифры
            //    op.User.RequireUniqueEmail = true; // уникальный email
            //}, mongoIdentityOptions =>
            //{
            //    mongoIdentityOptions.ConnectionString = config.MongoDb.ConnectionString + "/" + config.MongoDb.MongoDbIdentity;
            //}).AddDefaultTokenProviders();
        //}


            return builder;
        }
    }
}
