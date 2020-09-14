
using CarHealth.IdentityServer4.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.IdentityServer4.ViewModels
{
    public class ChangeRoleViewModel //Эта модель позволит управлять всеми ролями для одного пользователя 
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public List<Role> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }

        public ChangeRoleViewModel()
        {
            AllRoles = new List<Role>();
            UserRoles = new List<string>();
        }
    }
}
