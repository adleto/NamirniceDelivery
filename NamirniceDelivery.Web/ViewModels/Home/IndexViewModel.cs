using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Home
{
    public class IndexViewModel
    {
        public List<NamirnicaPodruznica> NamirnicaList { get; set; }
        public List<NamirniceDelivery.Data.Entities.Podruznica> PodruznicaList { get; set; }
    }
}
