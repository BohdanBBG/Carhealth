using CarHealth.Seed.Contexts;
using CarHealth.Seed.Models;
using CarHealth.Seed.Models.IdentityModels;
using CarHealth.Seed.Models.IdentityServer4Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Repositories.EFCoreDb
{
    public class EFCoreIdentitySeedRepository : IIdentitySeedRepository
    {
        private readonly IdentityServerContext _identityContex;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;


        public EFCoreIdentitySeedRepository(IdentityServerContext identityContex, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _identityContex = identityContex;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public bool IsClientCollectionEmpty()
        {
           return _identityContex.Clients.Any();
        }

        public bool IsIdentityResourceCollectionEmpty()
        {
            return _identityContex.IdentityResources.Any();
        }

        public bool IsApiResourceCollectionEmpty()
        {
            return _identityContex.ApiResources.Any();
        }

        public bool IsUserExist(string email)
        {
            var existing = _userManager.FindByNameAsync(email).GetAwaiter().GetResult();
            return existing != null;
        }

        public bool IsRoleExist(string name)
        {
            var existing = _roleManager.FindByNameAsync(name).GetAwaiter().GetResult();
            return existing != null;
        }



        public async Task<bool> AddClientAsync(ClientEntity client)
        {
            var result = await _identityContex.Clients.AddAsync(client);
            await _identityContex.SaveChangesAsync();

            return result.State == EntityState.Added;
        }

        public async Task<bool> AddIdentityResourceAsync(IdentityResourceEntity identityResource)
        {
            var result = await _identityContex.IdentityResources.AddAsync(identityResource);
            await _identityContex.SaveChangesAsync();

            return result.State == EntityState.Added;
        }

        public async Task<bool> AddApiResourceAsync(ApiResourceEntity apiResource)
        {
            var result = await _identityContex.ApiResources.AddAsync(apiResource);
            await _identityContex.SaveChangesAsync();

            return result.State == EntityState.Added;
        }

        public async Task<bool> AddUser(User user, string defaultPassword)
        {
            var existing = _userManager.FindByNameAsync(user.Email).GetAwaiter().GetResult();

            if (existing == null)
            {

                var result = await _userManager.CreateAsync(user, defaultPassword);

                if (result.Succeeded && user.Email == "admin1@gmail.com")
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }

                if (!result.Succeeded)
                {
                    var errorList = result.Errors.ToList();
                    throw new Exception(string.Join("; ", errorList));
                }
                else
                {
                    return true;
                }

            }

            return false;

        }

        public async Task<bool> AddRole(Role role)
        {
            var existing = _roleManager.FindByNameAsync(role.Name).GetAwaiter().GetResult();

            if (existing == null)
            {
                var result = _roleManager.CreateAsync(role);

                if (!result.Result.Succeeded)
                {
                    var errorList = result.Result.Errors.ToList();
                    throw new Exception(string.Join("; ", errorList));
                }

                return true;
            }

            return false;
        }
    }
}
