using CarHealth.IdentityServer4.Models;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.IdentityServer4.Stores.EFCoreStores
{
    public class EFCoreClientStore : IClientStore
    {
        private IdentityServerContext _identityDb { get; set; }
        public EFCoreClientStore(IdentityServerContext identityDb)
        {
            _identityDb = identityDb;
        }


        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = _identityDb.Clients.First(t => t.ClientId == clientId);
            client.MapDataFromEntity();
            return client.Client;

        }

    }
}
