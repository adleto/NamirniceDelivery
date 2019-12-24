using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NamirniceDelivery.Data.Entities
{
    public class KupljeneNamirnice
    {
        public int Id { get; set; }
        public virtual Transakcija Transakcija { get; set; }
        [ForeignKey("Transakcija")]
        public int TransakcijaId { get; set; }
        public decimal Cijena { get; set; }
        public int Kolicina { get; set; }
        public virtual Namirnica Namirnica { get; set; }
        [ForeignKey("Namirnica")]
        public int NamirnicaId { get; set; }
        [NotMapped]
        public decimal CijenaTotal { get {
                return Cijena * Kolicina;
            } 
        }
    }
}
