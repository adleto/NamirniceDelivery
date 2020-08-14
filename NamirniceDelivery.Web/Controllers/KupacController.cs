using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.ViewModels;
using NamirniceDelivery.Web.ViewModels.Kupac;
using NamirniceDelivery.Web.ViewModels.Shared;

namespace NamirniceDelivery.Web.Controllers
{
    public class KupacController : Controller
    {
        private static SignInManager<ApplicationUser> _signInManager;
        private readonly IKupac _kupacService;
        private readonly INamirnicaPodruznica _namirnicaPodruznicaService;
        private readonly IPodruznica _podruznicaService;
        private readonly IKorpaStavka _korpaStavkaService;
        private readonly ITransakcija _transakcijaService;
        private readonly IAkcijeTransakcija _akcijeTransakcijaService;

        public KupacController(SignInManager<ApplicationUser> signInManager, IKupac kupacService, INamirnicaPodruznica namirnicaPodruznicaService, IPodruznica podruznicaService, IKorpaStavka korpaStavkaService, ITransakcija transakcijaService, IAkcijeTransakcija akcijeTransakcijaService)
        {
            _signInManager = signInManager;
            _kupacService = kupacService;
            _namirnicaPodruznicaService = namirnicaPodruznicaService;
            _podruznicaService = podruznicaService;
            _korpaStavkaService = korpaStavkaService;
            _transakcijaService = transakcijaService;
            _akcijeTransakcijaService = akcijeTransakcijaService;
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
                PodruznicaList = _podruznicaService.GetPodruzniceForKupac(kupac),
                SpremljeneNamirniceList = KonvertujSpremljeneUNamirnice(_kupacService.GetSpremljeneNamirnice(kupac.Id)),
                SpremljenePodruzniceList = KonvertujSpremljeneUPodruznice(_kupacService.GetSpremljenePodruznice(kupac.Id))
            });
        }
        [Authorize(Roles="Kupac")]
        public IActionResult GetDataPodruznice()
        {
            var kupac = _kupacService.GetKupac(User.Identity.Name);
            return PartialView("_PodruznicePregledPartial", new PregledPodruznicaPartialViewModel
            {
                PodruznicaList = _podruznicaService.GetPodruzniceForKupac(kupac),
                SpremljenePodruzniceList = KonvertujSpremljeneUPodruznice(_kupacService.GetSpremljenePodruznice(kupac.Id))
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
        [HttpPost]
        [Authorize(Roles = "Kupac")]
        public IActionResult Kupi()
        {
            var kupac = _kupacService.GetKupac(User.Identity.Name);
            _transakcijaService.RealizujKupovine(_korpaStavkaService.GetNamirniceUKorpi(kupac));
            return RedirectToAction("NepotvrdjeneNarudzbe","Transakcija");
        }
        [Authorize(Roles = "Kupac")]
        public IActionResult BrzaKupovina(string[] namirnicaPodruznicaId, string[] kolicina)
        {
            var kupac = _kupacService.GetKupac(User.Identity.Name);
            for (int i = 0; i < namirnicaPodruznicaId.Count(); i++)
            {
                int brojNamirnica = int.Parse(kolicina[i]);
                if (brojNamirnica > 0)
                {
                    int idNamirnice = int.Parse(namirnicaPodruznicaId[i]);
                    _transakcijaService.BrzaKupovina(_namirnicaPodruznicaService.GetNamirnicaPodruznica(idNamirnice), brojNamirnica, kupac);
                }
            }
            return RedirectToAction("NepotvrdjeneNarudzbe", "Transakcija");
        }
        [Authorize(Roles = "Kupac")]
        public IActionResult DodajSpremljenuNamirnicu(int namirnicaPodruznicaId, string returnUrl = "")
        {
            var kupac = _kupacService.GetKupac(User.Identity.Name);
            _kupacService.DodajSpremljenuNamiricu(kupac.Id, namirnicaPodruznicaId);
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction(nameof(Index));
            }
            return Redirect(returnUrl);
        }
        [Authorize(Roles = "Kupac")]
        public IActionResult DodajSpremljenuPodruznicu(int podruznicaId, string returnUrl = "")
        {
            var kupac = _kupacService.GetKupac(User.Identity.Name);
            _kupacService.DodajSpremljenuPodruznicu(kupac.Id, podruznicaId);
            //return Ok("Ok");
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction(nameof(Index));
            }
            return Redirect(returnUrl);
        }
        [Authorize(Roles = "Kupac")]
        public IActionResult UkloniSpremljenuNamirnicu(int namirnicaPodruznicaId, string returnUrl = "")
        {
            var kupac = _kupacService.GetKupac(User.Identity.Name);
            _kupacService.UkloniSpremljenuNamiricu(kupac.Id, namirnicaPodruznicaId);
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction(nameof(Index));
            }
            return Redirect(returnUrl);
        }
        [Authorize(Roles = "Kupac")]
        public IActionResult UkloniSpremljenuPodruznicu(int podruznicaId, string returnUrl = "")
        {
            var kupac = _kupacService.GetKupac(User.Identity.Name);
            _kupacService.UkloniSpremljenuPodruznicu(kupac.Id, podruznicaId);
            //return Ok("Ok");
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction(nameof(Index));
            }
            return Redirect(returnUrl);
        }
        [Authorize(Roles = "Kupac")]
        public IActionResult SpremljeneNamirnice()
        {
            var kupac = _kupacService.GetKupac(User.Identity.Name);
            return View(new SpremljeneNamirniceViewModel
            {
                SpremljeneNamirniceList = KonvertujSpremljeneUNamirnice(_kupacService.GetSpremljeneNamirnice(kupac.Id)),
                SpremljenePodruzniceList = KonvertujSpremljeneUPodruznice(_kupacService.GetSpremljenePodruznice(kupac.Id))
            });
        }

        [Authorize(Roles = "Kupac")]
        public IActionResult SpremljenePodruznice()
        {
            var kupac = _kupacService.GetKupac(User.Identity.Name);
            return View(new SpremljenePodruzniceViewModel
            {
                SpremljenePodruzniceList = KonvertujSpremljeneUPodruznice(_kupacService.GetSpremljenePodruznice(kupac.Id))
            });
        }
        public IActionResult GetDataSpremljenePodruznice()
        {
            var kupac = _kupacService.GetKupac(User.Identity.Name);
            var spremljene = KonvertujSpremljeneUPodruznice(_kupacService.GetSpremljenePodruznice(kupac.Id));
            return PartialView("_PodruznicePregledPartial", new PregledPodruznicaPartialViewModel
            {
                PodruznicaList = spremljene,
                SpremljenePodruzniceList = spremljene
            });
        }
        [Authorize(Roles = "Kupac")]
        public IActionResult DostavaUspjesna(int transakcijaId, string returnUrl="")
        {
            var kupac = _kupacService.GetKupac(User.Identity.Name);
            _akcijeTransakcijaService.DostavaUspjela(transakcijaId, kupac);
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("ZavrseneTransakcije", "Transakcija");
            }
            return Redirect(returnUrl);
        }
        

        private List<NamirnicaPodruznica> KonvertujSpremljeneUNamirnice(List<KupacSpremljeneNamirnice> list)
        {
            return list
                .Select(ksn => _namirnicaPodruznicaService.GetNamirnicaPodruznica(ksn.NamirnicaPodruznicaId))
                .ToList();
        }
        private List<Podruznica> KonvertujSpremljeneUPodruznice(List<KupacSpremljenePodruznice> list)
        {
            return list
                .Select(ksp => _podruznicaService.GetPodruznica(ksp.PodruznicaId))
                .ToList();
        }

        [Authorize]
        public IActionResult Profil(string username)
        {
            KupacProfilViewModel model = _kupacService.GetKupacData(username);
            return View(model);
        }
    }
}