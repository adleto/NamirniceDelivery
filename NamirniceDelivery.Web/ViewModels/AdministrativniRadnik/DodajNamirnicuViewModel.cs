using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.AdministrativniRadnik
{
    public class DodajNamirnicuViewModel
    {
        [Required(ErrorMessage = "Cijena mora biti unesena.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Najmanja moguća cijena je {1}KM.")]
        public decimal? Cijena { get; set; }
        [Required(ErrorMessage = "Namirnica mora biti odabrana.")]
        [Display(Name = "Namirnica")]
        public int NamirnicaId { get; set; }
        [Required(ErrorMessage = "Količina na stanju mora biti unesena.")]
        [Range(1, double.MaxValue, ErrorMessage = "Najmanja moguća količina je {1}.")]
        [Display(Name = "Količina na stanju")]
        public int? KolicinaNaStanju { get; set; }
        public List<Namirnica> NamirnicaList { get; set; }
        [Display(Name = "Popust")]
        public int PopustId { get; set; }
        public List<Popust> PopustList { get; set; }
        public string ReturnUrl { get; set; }
    }
}
