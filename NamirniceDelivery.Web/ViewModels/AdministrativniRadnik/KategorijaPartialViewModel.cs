using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.AdministrativniRadnik
{
    public class KategorijaPartialViewModel
    {
        [Required]
        public int KategorijaId { get; set; }
        [Required(ErrorMessage = "Naziv kategorije mora biti unesen.")]
        public string Naziv { get; set; }
    }
}
