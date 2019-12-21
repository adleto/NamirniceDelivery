using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.AdministrativniRadnik
{
    public class EditKategorijaViewModel
    {
        public string ReturnUrl { get; set; }
        [Required]
        public int KategorijaId { get; set; }
        [Required(ErrorMessage = "Naziv kategorije mora biti unesen.")]
        public string Naziv { get; set; }
    }
}
