using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Podruznica
{
    public class PodruznicaVM
    {
        [Required(ErrorMessage ="Adresa mora biti unesena.")]
        public string Adresa { get; set; }
        [Required(ErrorMessage = "Opcina mora biti unesena.")]
        [Display(Name ="Općina")]
        public int OpcinaId { get; set; }
        public List<Opcina> OpcinaList { get; set; }
        [Required(ErrorMessage = "Naziv mora biti unesen.")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Opis mora biti unesen.")]
        public string Opis { get; set; }
        public int PodruznicaId { get; set; }
    }
}
