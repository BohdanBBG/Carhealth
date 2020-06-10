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

        public DbSet<Client> Clients { get; set; }
        public DbSet<IdentityResource> IdentityResources { get; set; }
        public DbSet<ApiResource> ApiResources { get; set; }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<ClientEntity>().HasKey(m => m.ClientId);
        //    builder.Entity<ApiResourceEntity>().HasKey(m => m.ApiResourceId);
        //    builder.Entity<IdentityResourceEntity>().HasKey(m => m.IdentityResourceId);
        //    base.OnModelCreating(builder);
        //}
    }
}
