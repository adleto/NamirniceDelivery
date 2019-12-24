using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NamirniceDelivery.Data.Entities
{
    public class NamirnicaVoznja
    {
        public int Id { get; set; }
        public virtual Voznja Voznja { get; set; }
        [ForeignKey("Voznja")]
        public int VoznjaId { get; set; }
        public virtual Namirnica Namirnica { get; set; }
        [ForeignKey("Namirnica")]
        public int NamirnicaId { get; set; }
        public int KolicinaPrevezena { get; set; }

    }
}
