using CarHealth.Seed.Models;
using CarHealth.Seed.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Repositories
{
    public class RoleInitializer
    {
        // добавляет в базу данных две роли - "admin" и "user", а также одного пользователя - администратора
        public static async Task InitializeAsync (UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin1@gmail.com";
            string user1Email = "user1@gmail.com";

            string password = "1234";

            if(await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new Role("Admin"));
            }

            if(await userManager.FindByNameAsync("User") == null)
            {
                await roleManager.CreateAsync(new Role("User"));
            }

            if( await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };

                var result = await userManager.CreateAsync(admin, password);

                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");

                }
            }

            if( await userManager.FindByNameAsync(user1Email) == null)
            {
                User user1 = new User { Email = user1Email, UserName = user1Email };

                var result = await userManager.CreateAsync(user1, password);

                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user1, "User");
                }
            }
        }
    }
}
