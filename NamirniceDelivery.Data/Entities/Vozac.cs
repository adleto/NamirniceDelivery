using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NamirniceDelivery.Data.Entities
{
    public class Vozac:ApplicationUser
    {
        public string KategorijaVozackeDozvole { get; set; }
        public virtual Vozilo Vozilo { get; set; }
        [ForeignKey("Vozilo")]
        public int? VoziloId { get; set; }
    }
}
