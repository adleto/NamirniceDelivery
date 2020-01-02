using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.AdministrativniRadnik
{
    public class NamirnicaListViewModel
    {
        public List<Namirnica> NamirnicaList { get; set; }
        public List<bool> Deletable { get; set; }
    }
}
