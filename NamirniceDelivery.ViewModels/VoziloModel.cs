using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NamirniceDelivery.ViewModels
{
    public class VoziloModel
    {
        public int? Id { get; set; }
        [Required]
        public string TipVozila { get; set; }
        [Required]
        public string RegistarskeOznake { get; set; }
        [Required]
        public string MarkaVozila { get; set; }
        public string VozacName { get; set; }
        public string VozacId { get; set; }
        public List<VozaciZaDisplay> Vozaci { get; set; }
    }

    public class VozaciZaDisplay
    {
        public string ImeIPrezime { get; set; }
        public string Id { get; set; }
    }
}
