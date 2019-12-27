using NamirniceDelivery.Web.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Transakcija
{
    public class IndexViewModel
    {
        public Data.Entities.Transakcija Transakcija { get; set; }
        public string ReturnUrl { get; set; }
        public ListType ListType { get; set; }
    }
}
