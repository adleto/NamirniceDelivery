using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NamirniceDelivery.Data.Entities
{
    public class KupacSpremljenePodruznice
    {
        public int Id { get; set; }
        public virtual Kupac Kupac { get; set; }
        [ForeignKey("Kupac")]
        public string KupacId { get; set; }
        public virtual Podruznica Podruznica { get; set; }
        [ForeignKey("Podruznica")]
        public int PodruznicaId { get; set; }
    }
}
