using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.IdentityServer4.ViewModels
{
    public class RegisterViewModel // будет представлять регистрирующегося пользователя
    {
        [Required]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Field {0} must have a minimum of {2} and a maximum of {1} characters.", MinimumLength = 4)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Different passwods")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        public string ReturnUrl { get; set; }

    }
}
