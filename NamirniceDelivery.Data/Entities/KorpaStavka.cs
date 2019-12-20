using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NamirniceDelivery.Data.Entities
{
    public class KorpaStavka
    {
        public int Id { get; set; }
        public virtual Kupac Kupac { get; set; }
        [ForeignKey("Kupac")]
        public string KupacId { get; set; }
        public int Kolicina { get; set; }
        public virtual NamirnicaPodruznica NamirnicaPodruznica { get; set; }
        [ForeignKey("NamirnicaPodruznica")]
        public int NamirnicaPodruznicaId { get; set; }
        [NotMapped]
        public decimal CijenaStavkeTotal { get {
                return Kolicina * NamirnicaPodruznica.CijenaSaPopustom;
            }
        }
    }
}
