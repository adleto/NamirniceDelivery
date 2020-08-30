using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Voznja
{
    public class IndexVM
    {
        public List<Row> VoznjaList { get; set; }
        public class Row
        {
            public int Id { get; set; }
            public bool PreuzetaRoba { get; set; }
            public bool ObavljenaVoznja { get; set; }
            public string VoznjaPocetak { get; set; }
            public string VoznjaKraj{ get; set; }

            public string PodruznicaPocetakNaziv { get; set; }
            public string PodruznicaKrajNaziv { get; set; }
            public string VozacIme { get; set; }
            public List<string> NamirnicaVoznjaNaziv { get; set; }
        }

    }
}
