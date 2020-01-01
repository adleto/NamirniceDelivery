using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.AdministrativniRadnik
{
    public class KategorijaListViewModel
    {
        public List<Kategorija> KategorijaList { get; set; }
        public List<bool> Deletable { get; set; }
    }
}
