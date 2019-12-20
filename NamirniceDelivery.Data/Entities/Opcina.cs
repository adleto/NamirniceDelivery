using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NamirniceDelivery.Data.Entities
{
    public class Opcina
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public virtual Kanton Kanton { get; set; }
        [ForeignKey("Kanton")]
        public int KantonId { get; set; }
    }
}
