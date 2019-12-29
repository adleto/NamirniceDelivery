using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Data.HelperModel
{
    public class TransakcijeNamirnica
    {
        [StringLength(64)]
        public string name { get; set; }

        public int? value { get; set; }
    }
}

