using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Podruznica
{
    public class IndexViewModel
    {
        public string ReturnUrl { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public string Adresa { get; set; }
        public Opcina Opcina { get; set; }
        public bool IsFavourite_ZaKupca { get; set; }
        public bool MozeKupovati_ZaKupca { get; set; }
        public List<NamirnicaPodruznica> NamirnicaList { get; set; }
        public List<NamirnicaPodruznica> SpremljeneNamirniceList { get; set; }
    }
}
