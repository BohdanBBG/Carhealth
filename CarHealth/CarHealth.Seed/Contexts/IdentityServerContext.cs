using CarHealth.Seed.Models;
using CarHealth.Seed.Models.IdentityServer4Models;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Contexts
{
    public class IdentityServerContext: DbContext
    {
        public IdentityServerContext(DbContextOptions<IdentityServerContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<ApiResourceEntity> ApiResources { get; set; }
        public DbSet<IdentityResourceEntity> IdentityResources { get; set; }


        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<ClientEntity>().HasKey(m => m.ClientId);
        //    builder.Entity<ApiResourceEntity>().HasKey(m => m.ApiResourceName);
        //    builder.Entity<IdentityResourceEntity>().HasKey(m => m.IdentityResourceName);
        //    base.OnModelCreating(builder);
        //}
    }
}
