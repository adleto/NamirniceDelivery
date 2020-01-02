using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.AdministrativniRadnik
{
    public class NamirnicaPodruznicaPartialViewModel
    {
        [Required]
        public int NamirnicaPodruznicaId { get; set; }
        public string Naziv { get; set; }
        public List<Namirnica> NamirnicaList { get; set; }
        [Required(ErrorMessage = "Vrsta mora biti odabrana.")]
        [Display(Name="Vrsta namirnice")]
        public int NamirnicaId { get; set; }
        
        [Required(ErrorMessage = "Cijena mora biti unesena.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Najmanja moguća cijena je {1}KM.")]
        public decimal? Cijena { get; set; }
        [Display(Name = "Popust")]
        public int PopustId { get; set; }
        [Required]
        public bool Aktivna { get; set; }
        [Required(ErrorMessage = "Količina na stanju mora biti unesena.")]
        [Range(1, double.MaxValue, ErrorMessage = "Najmanja moguća količina je {1}.")]
        [Display(Name = "Količina na stanju")]
        public int KolicinaNaStanju { get; set; }
        public List<Popust> PopustList { get; set; }
    }
}
