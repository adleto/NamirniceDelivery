using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NamirniceDelivery.Services.Interfaces;
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

        public TransakcijaController(IKupac kupacService, IAdministrativniRadnik administrativniRadnikService, ITransakcija transakcijaService, IPodruznica podruznicaService, INamirnicaPodruznica namirnicaPodruznicaService)
        {
            _kupacService = kupacService;
            _administrativniRadnikService = administrativniRadnikService;
            _transakcijaService = transakcijaService;
            _podruznicaService = podruznicaService;
            _namirnicaPodruznicaService = namirnicaPodruznicaService;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
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
    }
}