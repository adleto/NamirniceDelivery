using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Transakcija
{
    public class StatistikaViewModel
    {
        public decimal TotalVrijednost { get; set; }
        public Data.Entities.Transakcija NajvecaTransakcija { get; set; }
        public Tuple<string,int> NajNamirnica { get; set; }
    }
}
