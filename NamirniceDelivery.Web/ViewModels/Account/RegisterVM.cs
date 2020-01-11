using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Account
{
    public class RegisterVM
    {
        public string ErrorMessage { get; set; }
        [Required(ErrorMessage ="Korisničko ime mora biti uneseno.")]
        [Display(Name ="Korisničko ime")]
        public string Username { get; set; }
        public string ReturnUrl { get; set; }
        [Required(ErrorMessage = "Email adresa mora biti unesena.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "{0} mora imati barem {2} karaktera.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Ponovi lozinku")]
        [Compare("Password", ErrorMessage = "Lozinke se ne poklapaju.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Ime mora biti uneseno.")]
        public string Ime { get; set; }
        [Required(ErrorMessage = "Prezime mora biti uneseno.")]
        public string Prezime { get; set; }
        [Required(ErrorMessage = "Općina boravka mora biti odabrana.")]
        [Display(Name = "Općina boravka")]
        public int? OpcinaBoravkaId { get; set; }
        public List<Opcina> OpcinaList { get; set; }
        [Required(ErrorMessage = "Općina rođenja mora biti odabrana.")]
        [Display(Name = "Općina rođenja")]
        public int? OpcinaRodjenjaId { get; set; }
        [Required(ErrorMessage = "Adresa mora biti unesena.")]
        public string Adresa { get; set; }
    }
}
