using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Shared
{
    public class PregledTransakcijaPartialViewModel
    {
        public List<Data.Entities.Transakcija> TransakcijaList { get; set; }
        //narucene,prihvacene,zavrsene
        public ListType ListType { get; set; }
    }
}
