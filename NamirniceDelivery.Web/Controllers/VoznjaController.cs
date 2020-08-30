using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.ViewModels.Voznja;

namespace NamirniceDelivery.Web.Controllers
{

    public class VoznjaController : Controller
    {

        private readonly IVoznja _voznjaService;
        private readonly IVozac _vozacService;

        public VoznjaController(IVoznja voznjaService, IVozac vozacService)
        {
            _voznjaService = voznjaService;
            _vozacService = vozacService;
        }
        [Authorize(Roles = "Menadzer,Vozac")]
        public IActionResult Index()
        {
            if (HttpContext.User.IsInRole("Menadzer"))
            {
                return View(new IndexVM
                {
                    VoznjaList = _voznjaService.GetVoznje().Select(
                        v => new IndexVM.Row
                        {
                            Id = v.Id,
                            PreuzetaRoba = v.PreuzetaRoba,
                            ObavljenaVoznja = v.ObavljenaVoznja,
                        //NamirnicaVoznjaNaziv=v.NamirnicaVoznja,
                        PodruznicaKrajNaziv = v.PodruznicaKraj.Naziv,
                            PodruznicaPocetakNaziv = v.PodruznicaPocetak.Naziv,
                            VozacIme = v.Vozac.Ime,
                            VoznjaKraj = v.VoznjaKraj.ToString(),
                            VoznjaPocetak = v.VoznjaPocetak.ToString()

                        }).ToList()
                });
            }
            else
            {
                return View(new IndexVM
                {
                    VoznjaList = _voznjaService.GetVoznjeForVozacNeObavljenje(HttpContext.User.Identity.Name).Select(
                        v => new IndexVM.Row
                        {
                            Id = v.Id,
                            PreuzetaRoba = v.PreuzetaRoba,
                            ObavljenaVoznja = v.ObavljenaVoznja,
                            //NamirnicaVoznjaNaziv=v.NamirnicaVoznja,
                            PodruznicaKrajNaziv = v.PodruznicaKraj.Naziv,
                            PodruznicaPocetakNaziv = v.PodruznicaPocetak.Naziv,
                            VozacIme = v.Vozac.Ime,
                            VoznjaKraj = v.VoznjaKraj.ToString(),
                            VoznjaPocetak = v.VoznjaPocetak.ToString()
                        }).ToList()
                });
            }
        }
        [HttpGet]
        [Authorize(Roles ="Vozac")]
        public IActionResult PreuzmiRobu(int voznjaId)
        {
            _voznjaService.PreuzmiRobu(HttpContext.User.Identity.Name, voznjaId);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = "Vozac")]
        public IActionResult OznaciKaoZavrsenu(int voznjaId)
        {
            _voznjaService.OznaciKaoZavrsenu(HttpContext.User.Identity.Name, voznjaId);
            return RedirectToAction(nameof(Index));
        }
    }
}
