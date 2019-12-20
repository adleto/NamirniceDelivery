using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NamirniceDelivery.Data.Entities
{
    public class Podruznica
    {
        public int Id { get; set; }
        public string Adresa { get; set; }
        virtual public Opcina Opcina { get; set; }
        [ForeignKey("Opcina")]
        public int OpcinaId { get; set; }
        public string Opis { get; set; }
        public List<NamirnicaPodruznica> NamirnicaPodruznica { get; set; }
    }
}
