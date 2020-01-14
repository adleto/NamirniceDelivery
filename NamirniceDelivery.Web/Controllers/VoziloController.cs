using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.ViewModels.Vozilo;

namespace NamirniceDelivery.Web.Controllers
{
    public class VoziloController : Controller
    {
        private readonly IVozilo _voziloService;

        public VoziloController(IVozilo voziloService)
        {
            _voziloService = voziloService;
        }

        public IActionResult Index()
        {
            return View(new IndexVM { 
                VoziloList = _voziloService.GetVozila().Select(
                    v => new IndexVM.Row { 
                        MarkaVozila = v.MarkaVozila,
                        RegistarskeOznake = v.RegistarskeOznake,
                        TipVozila = v.TipVozila
                    }).ToList()
            });
        }
    }
}