using System;
using System.Collections.Generic;
using System.Text;

namespace NamirniceDelivery.Data.Entities
{
    public class Kupac:ApplicationUser
    {
        public string Adresa { get; set; }
        public virtual List<KupacSpremljeneNamirnice> SpreljeneNamirnice { get; set; }
        public virtual List<KupacSpremljenePodruznice> SpremljenePodruznice { get; set; }
        public virtual List<Transakcija> Transakcije { get; set; }
        public int RejtingKupac { get; set; }
    }
}
