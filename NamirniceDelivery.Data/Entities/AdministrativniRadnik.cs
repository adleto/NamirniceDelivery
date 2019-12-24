using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NamirniceDelivery.Data.Entities
{
    public class AdministrativniRadnik:ApplicationUser
    {
        public string JMBG { get; set; }
        public virtual List<Transakcija> Transakcije { get; set; }
        public virtual Podruznica Podruznica { get; set; }
        [ForeignKey("Podruznica")]
        public int? PodruznicaId { get; set; }
        public int RejtingRadnik { get; set; }
    }
}
