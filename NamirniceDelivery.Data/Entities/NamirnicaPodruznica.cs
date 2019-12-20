using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NamirniceDelivery.Data.Entities
{
    public class NamirnicaPodruznica
    {
        public int Id { get; set; }
        public virtual Namirnica Namirnica { get; set; }
        [ForeignKey("Namirnica")]
        public int NamirnicaId { get; set; }
        public virtual Podruznica Podruznica { get; set; }
        [ForeignKey("Podruznica")]
        public int PodruznicaId { get; set; }
        public decimal Cijena { get; set; }
        public bool Aktivna { get; set; }
        public int KolicinaNaStanju { get; set; }
        public virtual Popust Popust { get; set; }
        [ForeignKey("Popust")]
        public int? PopustId { get; set; }
        [NotMapped]
        public decimal CijenaSaPopustom { get {
                if (Popust != null)
                {
                    return Cijena - (Popust.Iznos * Cijena);
                }
                else
                {
                    return Cijena;
                }
        } }
    }
}
