using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.Models
{
    public class NamirnicaExcel
    {
        public string Broj { get; set; }
        public string Namirnica { get; set; }
        public string Cijena { get; set; }
        public string CijenaSaPopustom { get; set; }
        public string KolicinaNaStanju { get; set; }
    }
}
