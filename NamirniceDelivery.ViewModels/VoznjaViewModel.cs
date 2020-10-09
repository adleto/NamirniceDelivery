using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NamirniceDelivery.ViewModels
{
    public class VoznjaViewModel
    {
        public int? VoznjaId { get; set; }
        [Required]
        public string VozacId { get; set; }
        [Required]
        public int PodruznicaPocetakId { get; set; }
        [Required]
        public int PodruznicaKrajId { get; set; }
        public List<PodruznicaVM> PodruzniceList { get; set; }
        public List<VozaciZaDisplay> VozaciList { get; set; }
    }
}
