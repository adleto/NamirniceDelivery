using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.ViewModels;
using NamirniceDelivery.Web.ViewModels.Vozilo;

namespace NamirniceDelivery.Web.Controllers
{
    [Authorize(Roles = "Menadzer")]
    public class VoziloController : Controller
    {
        private readonly IVozilo _voziloService;
        private readonly IVozac _vozacService;

        public VoziloController(IVozilo voziloService, IVozac vozacService)
        {
            _voziloService = voziloService;
            _vozacService = vozacService;
        }

        public IActionResult Index()
        {
            return View(new IndexVM { 
                VoziloList = _voziloService.GetVozila().Select(
                    v => new IndexVM.Row { 
                        MarkaVozila = v.MarkaVozila,
                        RegistarskeOznake = v.RegistarskeOznake,
                        TipVozila = v.TipVozila,
                        Id = v.Id,
                        VozacImePrezime = v.Vozac?.Ime +" " +v.Vozac?.Prezime
                    }).ToList()
            });
        }
        public IActionResult Vozilo(int? id)
        {
            try
            {
                var vm = new VoziloModel
                {
                    Vozaci = _vozacService.GetVozaciSimple(),
                    Id = null
                };
                if (id != null)
                {
                    var v = _voziloService.GetVozilo((int)id);
                    vm.Id = id;
                    vm.MarkaVozila = v.MarkaVozila;
                    vm.RegistarskeOznake = v.RegistarskeOznake;
                    vm.TipVozila = v.TipVozila;
                    if(v.VozacId != null)
                    {
                        vm.VozacId = v.VozacId;
                    }
                }
                return View(vm);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public IActionResult Vozilo(VoziloModel model)
        {
            try
            {
                _voziloService.Add_EditVozilo(model.Id, model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult DeleteVozilo(int id)
        {
            try
            {
                _voziloService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
            
        }
    }
}