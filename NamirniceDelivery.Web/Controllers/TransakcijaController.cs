using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.Models;
using NamirniceDelivery.Web.ViewModels.Shared;
using NamirniceDelivery.Web.ViewModels.Transakcija;
using Newtonsoft.Json;

namespace NamirniceDelivery.Web.Controllers
{
    [Authorize(Roles="AdministrativniRadnik,Kupac")]
    public class TransakcijaController : Controller
    {
        private readonly IKupac _kupacService;
        private readonly IAdministrativniRadnik _administrativniRadnikService;
        private readonly ITransakcija _transakcijaService;
        private readonly IPodruznica _podruznicaService;
        private readonly INamirnicaPodruznica _namirnicaPodruznicaService;
        private readonly IAkcijeTransakcija _akcijeTransakcijaService;
        private static UserManager<ApplicationUser> _userManager;

        public TransakcijaController(IKupac kupacService, IAdministrativniRadnik administrativniRadnikService, ITransakcija transakcijaService, IPodruznica podruznicaService, INamirnicaPodruznica namirnicaPodruznicaService, IAkcijeTransakcija akcijeTransakcijaService, UserManager<ApplicationUser> userManager)
        {
            _kupacService = kupacService;
            _administrativniRadnikService = administrativniRadnikService;
            _transakcijaService = transakcijaService;
            _podruznicaService = podruznicaService;
            _namirnicaPodruznicaService = namirnicaPodruznicaService;
            _akcijeTransakcijaService = akcijeTransakcijaService;
            _userManager = userManager;
        }

        public IActionResult Index(int transakcijaId, string returnUrl = "")
        {
            var t = _transakcijaService.GetTansakcija(transakcijaId);
            if (User.IsInRole("Kupac"))
            {
                if(User.Identity.Name != t.Kupac.UserName)
                {
                    return RedirectToAction("Index", "Kupac");
                }
            }
            else if (User.IsInRole("AdministrativniRadnik"))
            {
                var radnik = _administrativniRadnikService.GetRadnik(User.Identity.Name);
                if (!t.NarudzbaPrihvacenaOdRadnika)
                {
                    if(t.PodruznicaId != radnik.PodruznicaId)
                    {
                        return RedirectToAction("Index", "AdministrativniRadnik");
                    }
                }
                else
                {
                    if(t.AdministrativniRadnik==null || t.AdministrativniRadnikId != radnik.Id)
                    {
                        return RedirectToAction("Index", "AdministrativniRadnik");
                    }
                }
            }
            var model = new IndexViewModel
            {
                Transakcija = t,
                ReturnUrl = returnUrl
            };
            if (t.DostavaUspjesna) model.ListType = ListType.zavrsene;
            else if (t.NarudzbaPrihvacenaOdRadnika) model.ListType = ListType.prihvacene;
            else model.ListType = ListType.narucene;
            return View(model);
        }
        public IActionResult NepotvrdjeneNarudzbe()
        {
            var v = new NarudzbeViewModel();
            if (User.IsInRole("Kupac"))
            {
                var kupac = _kupacService.GetKupac(User.Identity.Name);
                v.TransakcijaList = _transakcijaService.GetNepotvrdjeneTransakcijeForKupac(kupac);
            }
            else if (User.IsInRole("AdministrativniRadnik"))
            {
                var radnik = _administrativniRadnikService.GetRadnik(User.Identity.Name);
                v.TransakcijaList = _transakcijaService.GetNepotvrdjeneTransakcijeForPodruznica(radnik.Podruznica);
            }
            return View(v);
        }
        public IActionResult DostaveUToku()
        {
            var v = new NarudzbeViewModel();
            if (User.IsInRole("Kupac"))
            {
                var kupac = _kupacService.GetKupac(User.Identity.Name);
                v.TransakcijaList = _transakcijaService.GetTransakcijeUTokuForKupac(kupac);
            }
            else if (User.IsInRole("AdministrativniRadnik"))
            {
                var radnik = _administrativniRadnikService.GetRadnik(User.Identity.Name);
                v.TransakcijaList = _transakcijaService.GetTransakcijeUTokuForRadnik(radnik);
            }
            return View(v);
        }
        public IActionResult ZavrseneTransakcije()
        {
            var v = new NarudzbeViewModel();
            if (User.IsInRole("Kupac"))
            {
                var kupac = _kupacService.GetKupac(User.Identity.Name);
                v.TransakcijaList = _transakcijaService.GetZavrseneTransakcijeForKupac(kupac);
            }
            else if (User.IsInRole("AdministrativniRadnik"))
            {
                var radnik = _administrativniRadnikService.GetRadnik(User.Identity.Name);
                v.TransakcijaList = _transakcijaService.GetZavrseneTransakcijeForRadnik(radnik);
            }
            return View(v);
        }
        public IActionResult OstaviPozitivanDojam(int transakcijaId, string returnUrl="")
        {
            var transakcija = _transakcijaService.GetTansakcija(transakcijaId);
            if (transakcija.DostavaUspjesna)
            {
                if(transakcija.Kupac.UserName == User.Identity.Name ||
                    transakcija.AdministrativniRadnik.UserName == User.Identity.Name)
                {
                    if (User.IsInRole("Kupac"))
                    {
                        if (!transakcija.KupacOstavioDojam)
                        {
                            _akcijeTransakcijaService.KupacOstaviPozitivan(transakcijaId);
                        }
                    }
                    else if (User.IsInRole("AdministrativniRadnik"))
                    {
                        if (!transakcija.RadnikOstavioDojam)
                        {
                            _akcijeTransakcijaService.RadnikOstaviPozitivan(transakcijaId);
                        }
                    }

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        //return Redirect(returnUrl);
                    }
                    return RedirectToAction(nameof(ZavrseneTransakcije));
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult OstaviNegativanDojam(int transakcijaId, string returnUrl = "")
        {
            var transakcija = _transakcijaService.GetTansakcija(transakcijaId);
            if (transakcija.DostavaUspjesna)
            {
                if (transakcija.Kupac.UserName == User.Identity.Name ||
                    transakcija.AdministrativniRadnik.UserName == User.Identity.Name)
                {
                    if (User.IsInRole("Kupac"))
                    {
                        if (!transakcija.KupacOstavioDojam)
                        {
                            _akcijeTransakcijaService.KupacOstaviNegativan(transakcijaId);
                        }
                    }
                    else if (User.IsInRole("AdministrativniRadnik"))
                    {
                        if (!transakcija.RadnikOstavioDojam) {
                            _akcijeTransakcijaService.RadnikOstaviNegativan(transakcijaId);
                        }
                    }

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        //return Redirect(returnUrl);
                    }
                    return RedirectToAction(nameof(ZavrseneTransakcije));
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Statistika()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var transakcije = _transakcijaService.GetTransakcije(user);
            List<DataPoint> dataPoints = new List<DataPoint>();

            for (int i = 0; i < transakcije.Count; i++)
            {
                dataPoints.Add(new DataPoint(transakcije[i].DatumIniciranjaTransakcije.ToString(), transakcije[i].IznosTotal));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);


            var v = new StatistikaViewModel
            {
                TotalVrijednost = _transakcijaService.GetTotalProtok(user),
                NajvecaTransakcija = _transakcijaService.GetNajvecaTransakcija(user),
                NajNamirnica = _transakcijaService.GetNajNamirnica(user),
                NajPartner = _transakcijaService.GetNajPartner(user)
            };
            return View(v);
        }
    }
}