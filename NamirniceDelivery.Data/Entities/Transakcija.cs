using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NamirniceDelivery.Data.Entities
{
    public class Transakcija
    {
        public int Id { get; set; }
        public bool DostavaUspjesna { get; set; }
        public DateTime DatumUspjesneDostave { get; set; }
        public bool NarudzbaPrihvacenaOdRadnika { get; set; }
        public DateTime DatumPrihvacanjaNarudzbe { get; set; }
        public bool RadnikOstavioDojam { get; set; }
        public bool KupacOstavioDojam { get; set; }
        public virtual List<KupljeneNamirnice> KupljeneNamirnice { get; set; }
        public DateTime DatumIniciranjaTransakcije { get; set; }
        public AdministrativniRadnik AdministrativniRadnik { get; set; }
        [ForeignKey("AdministrativniRadnik")]
        public string AdministrativniRadnikId { get; set; }
        public Kupac Kupac { get; set; }
        [ForeignKey("Kupac")]
        public string KupacId { get; set; }
        public virtual TipTransakcije TipTransakcije { get; set; }
        [ForeignKey("TipTransakcije")]
        public int TipTransakcijeId { get; set; }
        public string DojamRadnik { get; set; }
        public string DojamKupac { get; set; }
        [NotMapped]
        public decimal IznosTotal { get {
                decimal total = 0;
                foreach(var k in KupljeneNamirnice)
                {
                    total += k.CijenaTotal;
                }
                return total;
            } 
        }
    }
}
