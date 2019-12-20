using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.AdministrativniRadnik
{
    public class KreirajKategorijuViewModel
    {
        [Required(ErrorMessage = "Naziv kategorije mora biti unesen.")]
        public string Naziv { get; set; }
        public string ReturnUrl { get; set; }
    }
}
