using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.AdministrativniRadnik
{
    public class EditNamirnicaViewModel
    {
        public string ReturnUrl { get; set; }
        [Required]
        public int NamirnicaId { get; set; }
        [Required(ErrorMessage = "Kategorija mora biti odabrana.")]
        [Display(Name="Kategorija")]
        public int KategorijaId { get; set; }
        public List<Kategorija> KategorijaList { get; set; }
        [Required(ErrorMessage = "Naziv vrste namirnice mora biti unesen.")]
        public string Naziv { get; set; }
    }
}
