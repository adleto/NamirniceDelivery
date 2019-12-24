using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.ViewModels.Kupac;

namespace NamirniceDelivery.Web.Controllers
{
    public class KupacController : Controller
    {
        private static SignInManager<ApplicationUser> _signInManager;
        private readonly IKupac _kupacService;
        private readonly INamirnicaPodruznica _namirnicaPodruznicaService;
        private readonly IPodruznica _podruznicaService;

        public KupacController(SignInManager<ApplicationUser> signInManager, IKupac kupacService, INamirnicaPodruznica namirnicaPodruznicaService, IPodruznica podruznicaService)
        {
            _signInManager = signInManager;
            _kupacService = kupacService;
            _namirnicaPodruznicaService = namirnicaPodruznicaService;
            _podruznicaService = podruznicaService;
        }

        public async Task<IActionResult> DemoLogin()
        {
            await _signInManager.PasswordSignInAsync("testKupac", "password", false, lockoutOnFailure: true);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles="Kupac")]
        public IActionResult Index()
        {
            var kupac = _kupacService.GetKupac(User.Identity.Name);
            return View(new IndexViewModel { 
                Username = kupac.UserName,
                NamirnicaList = _namirnicaPodruznicaService.GetNamirniceForKupac(kupac),
                PodruznicaList = _podruznicaService.GetPodruzniceForKupac(kupac)
            });
        }
    }
}