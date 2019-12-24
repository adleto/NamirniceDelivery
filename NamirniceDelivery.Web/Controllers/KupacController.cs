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
        private readonly IKorpaStavka _korpaStavkaService;

        public KupacController(SignInManager<ApplicationUser> signInManager, IKupac kupacService, INamirnicaPodruznica namirnicaPodruznicaService, IPodruznica podruznicaService, IKorpaStavka korpaStavkaService)
        {
            _signInManager = signInManager;
            _kupacService = kupacService;
            _namirnicaPodruznicaService = namirnicaPodruznicaService;
            _podruznicaService = podruznicaService;
            _korpaStavkaService = korpaStavkaService;
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
        [Authorize(Roles = "Kupac")]
        [HttpPost]
        public IActionResult KorpaDodaj(string[] namirnicaPodruznicaId, string[] kolicina)
        {
            var kupac = _kupacService.GetKupac(User.Identity.Name);
            for (int i = 0; i < namirnicaPodruznicaId.Count(); i++)
            {
                if (int.TryParse(kolicina[i], out int brojNamirnica))
                {
                    if (brojNamirnica > 0)
                    {
                        _korpaStavkaService.DodajUKorpu(int.Parse(namirnicaPodruznicaId[i]), brojNamirnica, kupac);
                    }
                }
            }
            return RedirectToAction(nameof(Korpa));
        }
        [Authorize(Roles = "Kupac")]
        public IActionResult Korpa()
        {
            var kupac = _kupacService.GetKupac(User.Identity.Name);
            return View(new KorpaViewModel
            {
                NamirniceUKorpiList = _korpaStavkaService.GetNamirniceUKorpi(kupac),
                TotalCijena = _korpaStavkaService.GetTotalCijena(kupac)
            });
        }
        [Authorize(Roles = "Kupac")]
        public IActionResult UkloniIzKorpe(int korpaStavkaId)
        {
            var kupac = _kupacService.GetKupac(User.Identity.Name);
            _korpaStavkaService.UkloniStavku(korpaStavkaId, kupac);
            return RedirectToAction(nameof(Korpa));
        }
    }
}