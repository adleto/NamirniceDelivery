using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.AdministrativniRadnik
{
    public class EditNamirnicaPodruznicaViewModel
    {
        public string ReturnUrl { get; set; }
        public string Naziv { get; set; }
        [Required]
        public int NamirnicaPodruznicaId { get; set; }
        [Required(ErrorMessage = "Cijena mora biti unesena.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Najmanja moguća količina je {1}KM.")]
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
