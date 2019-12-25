using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.AdministrativniRadnik
{
    public class PregledNamirnicaViewModel
    {
        public string ReturnUrl { get; set; }
        public List<Namirnica> NamirnicaList { get; set; }
        public List<bool> Deletable { get; set; }
        public int KategorijaId { get; set; }
        public List<Kategorija> KategorijaList { get; set; }
    }
}
