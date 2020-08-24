using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NamirniceDelivery.ViewModels
{
    public class AdminRadnikViewModel
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
        public int OpcinaIdRodjenja { get; set; }
        [Required]
        public int OpcinaIdBoravka { get; set; }
        [Required]
        public string JMBG { get; set; }
        public List<OpcinaVM> OpcineList { get; set; }
        [Required]
        public int PodruznicaId { get; set; }
        public List<PodruznicaVM> PodruznicaList { get; set; }
    }

    public class PodruznicaVM
    {
        public string Naziv { get; set; }
        public int Id { get; set; }
    }

    public class OpcinaVM
    {
        public string Naziv { get; set; }
        public int Id { get; set; }
    }
}
