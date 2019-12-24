using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Kupac
{
    public class KorpaViewModel
    {
        public List<KorpaStavka> NamirniceUKorpiList { get; set; }
        public decimal TotalCijena { get; set; }
    }
}
