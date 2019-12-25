using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Shared
{
    public class PregledNamirnicaPartialViewModel
    {
        public List<NamirnicaPodruznica> SpremljeneNamirniceList { get; set; }
        public List<NamirnicaPodruznica> NamirnicaPodruznicaList { get; set; }
    }
}
