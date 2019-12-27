using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.ViewModels.Shared;
using NamirniceDelivery.Web.ViewModels.Transakcija;

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

        public TransakcijaController(IKupac kupacService, IAdministrativniRadnik administrativniRadnikService, ITransakcija transakcijaService, IPodruznica podruznicaService, INamirnicaPodruznica namirnicaPodruznicaService, IAkcijeTransakcija akcijeTransakcijaService)
        {
            _kupacService = kupacService;
            _administrativniRadnikService = administrativniRadnikService;
            _transakcijaService = transakcijaService;
            _podruznicaService = podruznicaService;
            _namirnicaPodruznicaService = namirnicaPodruznicaService;
            _akcijeTransakcijaService = akcijeTransakcijaService;
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
                        _akcijeTransakcijaService.KupacOstaviPozitivan(transakcijaId);
                    }
                    else if (User.IsInRole("AdministrativniRadnik"))
                    {
                        //dojam radnik
                    }

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
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
                        _akcijeTransakcijaService.KupacOstaviNegativan(transakcijaId);
                    }
                    else if (User.IsInRole("AdministrativniRadnik"))
                    {
                        //dojam radnik
                    }

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction(nameof(ZavrseneTransakcije));
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}