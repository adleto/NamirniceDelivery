using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Account
{
    public class LoginVM
    {
        public string ReturnUrl { get; set; }
        public string ErrorMessage { get; set; }
        [Required(ErrorMessage = "Korisničko ime mora biti uneseno.")]
        [Display(Name = "Korisničko ime")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Lozinka mora biti unesena.")]
        [Display(Name = "Lozinka")]
        public string Password { get; set; }

        [Display(Name = "Zapamti me?")]
        public bool RememberMe { get; set; }
    }
}
