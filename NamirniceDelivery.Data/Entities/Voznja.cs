using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NamirniceDelivery.Data.Entities
{
    public class Voznja
    {
        public int Id { get; set; }
        public bool PreuzetaRoba { get; set; }
        public bool ObavljenaVoznja { get; set; }
        public DateTime VoznjaPocetak { get; set; }
        public DateTime VoznjaKraj { get; set; }
        public virtual Podruznica PodruznicaPocetak { get; set; }
        [ForeignKey("Podruznica")]
        public int PodruznicaPocetakId { get; set; }
        public virtual Podruznica PodruznicaKraj { get; set; }
        [ForeignKey("Podruznica")]
        public int PodruznicaKrajId { get; set; }
        public virtual Vozac Vozac { get; set; }
        [ForeignKey("Vozac")]
        public string VozacId { get; set; }
        public virtual List<NamirnicaVoznja> NamirnicaVoznja { get; set; }
    }
}
