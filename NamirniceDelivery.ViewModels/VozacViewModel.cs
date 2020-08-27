using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NamirniceDelivery.ViewModels
{
    public class VozacViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Ime { get; set; }
        [Required]
        public string Prezime { get; set; }
        [Required]
        public string KategorijaVozackeDozvole { get; set; }
        [Required]
        public int OpcinaIdRodjenja { get; set; }
        [Required]
        public int OpcinaIdBoravka { get; set; }
       
        public List<OpcinaVM> OpcineList { get; set; }
        [Required]
        public int VoziloId { get; set; }
        public List<VoziloVM> VoziloList { get; set; }
    }
    public class VoziloVM
    {
        public string Naziv { get; set; }
        public int Id { get; set; }
    }

   
}
