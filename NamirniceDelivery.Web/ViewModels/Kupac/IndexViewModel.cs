using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Kupac
{
    public class IndexViewModel
    {
        public string Username { get; set; }
        public List<NamirnicaPodruznica> NamirnicaList { get; set; }
        public List<Podruznica> PodruznicaList { get; set; }
    }
}
