using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Shared
{
    public class PregledTransakcijaPartialViewModel
    {
        public List<Transakcija> TransakcijaList { get; set; }
        //narucene,prihvacene,zavrsene
        public string ListType { get; set; }
    }
}
