using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Vozilo
{
    public class IndexVM
    {
        public List<Row> VoziloList { get; set; }

        public class Row
        {
            public int Id { get; set; }
            public string TipVozila { get; set; }
            public string RegistarskeOznake { get; set; }
            public string MarkaVozila { get; set; }
            public string VozacImePrezime { get; set; }
        }
    }
}
