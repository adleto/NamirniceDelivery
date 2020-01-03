using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Shared
{
    public class PregledPodruznicaPartialViewModel
    {
        public List<Data.Entities.Podruznica> SpremljenePodruzniceList { get; set; }
        public List<Data.Entities.Podruznica> PodruznicaList { get; set; }
    }
}
