using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NamirniceDelivery.Data.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public virtual Opcina OpcinaBoravka { get; set; }
        [ForeignKey("Opcina")]
        public int? OpcinaBoravkaId { get; set; }
        public virtual Opcina OpcinaRodjenja { get; set; }
        [ForeignKey("Opcina")]
        public int? OpcinaRodjenjaId { get; set; }
    }
}
