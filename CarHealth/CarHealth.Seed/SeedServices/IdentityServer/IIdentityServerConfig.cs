using Carhealth.Seed;
using CarHealth.Seed.Models;
using CarHealth.Seed.Models.IdentityModels;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.SeedServices.IdentityServer
{
    public interface IIdentityServerConfig
    {
        string DefaultUserPassword { get; }

        IEnumerable<IdentityResource> GetIdentityResources();
        IEnumerable<ApiResource> GetApiResources();
        IEnumerable<Client> GetClients(IConfiguration config);


        List<Role> GetInitialIdentityRoles();
        List<User> GetInitialdentityUsers();
    }
}
