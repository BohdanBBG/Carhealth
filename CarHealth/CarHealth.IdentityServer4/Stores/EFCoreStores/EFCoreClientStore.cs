using CarHealth.IdentityServer4.Models;
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
        private IdentityContex _identityDb { get; set; }
        public EFCoreClientStore(IdentityContex identityDb)
        {
            _identityDb = identityDb;
        }


        public async Task<Client> FindClientByIdAsync(string clientId) => await _identityDb.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);

    }
}
