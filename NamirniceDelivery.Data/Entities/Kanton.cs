using System;
using System.Collections.Generic;
using System.Text;

namespace NamirniceDelivery.Data.Entities
{
    public class Kanton
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Oznaka { get; set; }
        public virtual List<Opcina> Opcine { get; set; }
    }
}
