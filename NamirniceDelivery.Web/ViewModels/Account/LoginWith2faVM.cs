using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Account
{
    public class LoginWith2faVM
    {
        public string StatusMessage { get; set; }
        [Required(ErrorMessage = "Kod mora biti unesen.")]
        [DataType(DataType.Text)]
        [Display(Name = "2FA kod")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "Zapamti browser")]
        public bool RememberMachine { get; set; }
        public bool RememberMe { get; set; }
    }
}
