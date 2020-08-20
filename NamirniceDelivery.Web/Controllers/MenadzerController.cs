using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Additional;
using NamirniceDelivery.Services.Interfaces;

namespace NamirniceDelivery.Web.Controllers
{
    
    public class MenadzerController : Controller
    {
        private static SignInManager<ApplicationUser> _signInManager;
        private readonly IKupac _kupacService;

        public MenadzerController(SignInManager<ApplicationUser> signInManager, IKupac kupacService)
        {
            _signInManager = signInManager;
            _kupacService = kupacService;
        }

        public async Task<IActionResult> DemoLogin()
        {
            await _signInManager.PasswordSignInAsync("menadzerMain", "password", false, lockoutOnFailure: true);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Menadzer")]
        public IActionResult Index()
        {
            return RedirectToAction("PregledPodruznica","Podruznica");
        }
        [Authorize(Roles="Menadzer")]
        public IActionResult SMSAlert()
        {
            var kupci = _kupacService.GetKupciZaSMSObavijest();

            NexmoSend.PodsjetiKupce(kupci);
            foreach (var k in kupci)
            {
                _kupacService.SMSObavjestPoslana(k);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}