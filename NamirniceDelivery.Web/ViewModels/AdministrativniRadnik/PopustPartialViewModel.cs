using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.AdministrativniRadnik
{
    public class PopustPartialViewModel
    {
        [Required]
        public int PopustId { get; set; }
        [Required(ErrorMessage = "Opis popusta mora biti unesen.")]
        public string Opis { get; set; }
        [Required(ErrorMessage = "Iznos popusta mora biti unesen.")]
        [Range(1, 99, ErrorMessage = "Popust može biti najviše {2}%, a najmanje {1}%.")]
        public decimal? Iznos { get; set; }
    }
}
