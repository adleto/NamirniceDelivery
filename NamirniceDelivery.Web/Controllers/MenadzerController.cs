using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Additional;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.ViewModels;
using NamirniceDelivery.Web.Models;
using OfficeOpenXml;

namespace NamirniceDelivery.Web.Controllers
{

    public class MenadzerController : Controller
    {
        private static SignInManager<ApplicationUser> _signInManager;
        private readonly IKupac _kupacService;
        private readonly IAdministrativniRadnik _administrativniRadniKService;
        private readonly IVozac _vozacService;
        private readonly IOpcina _opcinaService;
        private readonly IPodruznica _podruznicaService;
        private readonly IVozilo _voziloService;
        private readonly IVoznja _voznjaService;




        public MenadzerController(SignInManager<ApplicationUser> signInManager, IKupac kupacService, IAdministrativniRadnik administrativniRadniKService, IVozac vozacService, IOpcina opcinaService, IPodruznica podruznicaService, IVozilo voziloService, IVoznja voznjaService)
        {
            _signInManager = signInManager;
            _kupacService = kupacService;
            _administrativniRadniKService = administrativniRadniKService;
            _vozacService = vozacService;
            _opcinaService = opcinaService;
            _podruznicaService = podruznicaService;
            _voziloService = voziloService;
            _voznjaService = voznjaService;
        }

        public async Task<IActionResult> DemoLogin()
        {
            await _signInManager.PasswordSignInAsync("menadzerMain", "password", false, lockoutOnFailure: true);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Menadzer")]
        public IActionResult Index()
        {
            return RedirectToAction("PregledPodruznica", "Podruznica");
        }
        [Authorize(Roles = "Menadzer")]
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
        [Authorize(Roles = "Menadzer")]
        public IActionResult PregledAdmin()
        {
            return View(_administrativniRadniKService.Get());
        }
        [Authorize(Roles = "Menadzer")]
        public IActionResult Radnik(string radnikId = "")
        {
            var opcine = _opcinaService.GetOpcine();
            var podruznice = _podruznicaService.GetPodruznice();
            var myListPodruznice = new List<PodruznicaVM>();
            var myList = new List<OpcinaVM>();
            foreach (var o in opcine) myList.Add(new OpcinaVM { Id = o.Id, Naziv = o.Naziv });
            foreach (var p in podruznice) myListPodruznice.Add(new PodruznicaVM { Id = p.Id, Naziv = p.Naziv });
            var vm = new AdminRadnikViewModel
            {
                OpcineList = myList,
                PodruznicaList = myListPodruznice
            };
            if (radnikId != "")
            {
                var r = _administrativniRadniKService.GetRadnikById(radnikId);
                vm.Id = r.Id;
                vm.Ime = r.Ime;
                vm.JMBG = r.JMBG.ToString();
                vm.OpcinaIdBoravka = (int)r.OpcinaBoravkaId;
                vm.PodruznicaId = (int)r.PodruznicaId;
                vm.OpcinaIdRodjenja = (int)r.OpcinaRodjenjaId;
                vm.Prezime = r.Prezime;
                vm.Username = r.UserName;
            }
            return View(vm);
        }
        [HttpPost]
        [Authorize(Roles = "Menadzer")]
        public async Task<IActionResult> Radnik(AdminRadnikViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _administrativniRadniKService.Radnik(model);
                return RedirectToAction(nameof(PregledAdmin));
            }

            var opcine = _opcinaService.GetOpcine();
            var podruznice = _podruznicaService.GetPodruznice();
            var myListPodruznice = new List<PodruznicaVM>();
            var myList = new List<OpcinaVM>();
            foreach (var o in opcine) myList.Add(new OpcinaVM { Id = o.Id, Naziv = o.Naziv });
            foreach (var p in podruznice) myListPodruznice.Add(new PodruznicaVM { Id = p.Id, Naziv = p.Naziv });
            model.OpcineList = myList;
            model.PodruznicaList = myListPodruznice;
            return View(model);

        }
        [Authorize(Roles = "Menadzer")]
        public IActionResult DeleteRadnik(string radnikId)
        {
            _administrativniRadniKService.Deactivate(radnikId);
            return RedirectToAction(nameof(PregledAdmin));
        }

        [Authorize(Roles = "Menadzer")]
        public IActionResult PregledVozaca()
        {
            var vm = _vozacService.Get();
            return View(vm);
        }
        [Authorize(Roles = "Menadzer")]
        public IActionResult DeleteVozac(string vozacId)
        {
            _vozacService.Deactivate(vozacId);
            return RedirectToAction(nameof(PregledVozaca));
        }
        [Authorize(Roles = "Menadzer")]
        public IActionResult Vozac(string vozacId = "")
        {
            var opcine = _opcinaService.GetOpcine();
            var vozila = _voziloService.GetVozila();
            var myList = new List<OpcinaVM>();
            var myListVozila = new List<VoziloVM>();

            foreach (var o in opcine) myList.Add(new OpcinaVM { Id = o.Id, Naziv = o.Naziv });
            foreach (var v in vozila) myListVozila.Add(new VoziloVM { Id = v.Id, Naziv = v.MarkaVozila });
            var vm = new VozacViewModel
            {
                OpcineList = myList,
               VoziloList = myListVozila
            };
            if (vozacId != "")
            {
                var r = _vozacService.GetVozacById(vozacId);
                vm.Id = r.Id;
                vm.Ime = r.Ime;
                vm.KategorijaVozackeDozvole = r.KategorijaVozackeDozvole;
                vm.OpcinaIdBoravka = (int)r.OpcinaBoravkaId;
                vm.OpcinaIdRodjenja = (int)r.OpcinaRodjenjaId;
                vm.Prezime = r.Prezime;
                vm.Username = r.UserName;
            }
            return View(vm);
        }
        [Authorize(Roles = "Menadzer")]
        [HttpPost]
        public async Task<IActionResult> Vozac(VozacViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _vozacService.Vozac(model);
                return RedirectToAction(nameof(PregledVozaca));
            }

            var opcine = _opcinaService.GetOpcine();
            var vozila = _voziloService.GetVozila();
            var myListVozila = new List<VoziloVM>();
            var myList = new List<OpcinaVM>();
            foreach (var o in opcine) myList.Add(new OpcinaVM { Id = o.Id, Naziv = o.Naziv });
            foreach (var v in vozila) myListVozila.Add(new VoziloVM { Id = v.Id, Naziv = v.MarkaVozila });
            model.OpcineList = myList;
            model.VoziloList = myListVozila;
            return View(model);

        }
        


    }
}