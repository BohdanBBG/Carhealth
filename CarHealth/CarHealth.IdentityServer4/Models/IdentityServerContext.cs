using CarHealth.IdentityServer4.Models.IdentityServer4Models;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.IdentityServer4.Models
{
    public class IdentityServerContext: DbContext
    {
        public IdentityServerContext(DbContextOptions<IdentityServerContext> options)
          : base(options)
        {
        }

        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<ApiResourceEntity> ApiResources { get; set; }
        public DbSet<IdentityResourceEntity> IdentityResources { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ClientEntity>().HasKey(m => m.Id);
            builder.Entity<ApiResourceEntity>().HasKey(m => m.Id);
            builder.Entity<IdentityResourceEntity>().HasKey(m => m.Id);
            base.OnModelCreating(builder);
        }
    }
}
