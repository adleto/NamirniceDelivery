using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NamirniceDelivery.Data.Entities
{
    public class Namirnica
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public virtual Kategorija Kategorija { get; set; }
        [ForeignKey("Kategorija")]
        public int KategorijaId { get; set; }
    }
}
