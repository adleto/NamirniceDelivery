using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Account
{
    public class Enable2FAVM
    {
        public string SharedKey { get; set; }
        public string AuthenticatorUri { get; set; }
        [Required(ErrorMessage ="Kod mora biti unesen.")]
        [DataType(DataType.Text)]
        [Display(Name = "Verifikacijski kod")]
        public string VerificationCode { get; set; }
        public string StatusMessage { get; set; }
    }
}
