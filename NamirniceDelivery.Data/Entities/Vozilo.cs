using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NamirniceDelivery.Data.Entities
{
    public class Vozilo
    {
        public int Id { get; set; }
        public string TipVozila { get; set; }
        public string RegistarskeOznake { get; set; }
        public string MarkaVozila { get; set; }
        public virtual Vozac Vozac { get; set; }
        [ForeignKey("Vozac")]
        public string VozacId { get; set; }
    }
}
