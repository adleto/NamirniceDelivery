using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.ViewModels;
using NamirniceDelivery.Web.Models;
using NamirniceDelivery.Web.ViewModels.Voznja;
using OfficeOpenXml;

namespace NamirniceDelivery.Web.Controllers
{

    public class VoznjaController : Controller
    {

        private readonly IVoznja _voznjaService;
        private readonly IVozac _vozacService;
        private readonly IPodruznica _podruznicaService;

        public VoznjaController(IVoznja voznjaService, IVozac vozacService, IPodruznica podruznicaService)
        {
            _voznjaService = voznjaService;
            _vozacService = vozacService;
            _podruznicaService = podruznicaService;
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
                            VoznjaKraj = v.VoznjaKraj,
                            VoznjaPocetak = v.VoznjaPocetak

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
                            VoznjaKraj = v.VoznjaKraj,
                            VoznjaPocetak = v.VoznjaPocetak
                        }).ToList()
                });
            }
        }
        [HttpGet]
        [Authorize(Roles = "Vozac")]
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
        [HttpGet]
        [Authorize(Roles = "Menadzer")]
        public IActionResult Voznja(int? voznjaId = null)
        {
            try
            {
                var podruznice = _podruznicaService.GetPodruznice();
                var myListPodruznice = new List<PodruznicaVM>();
                foreach (var p in podruznice) myListPodruznice.Add(new PodruznicaVM { Id = p.Id, Naziv = p.Naziv });
                var vm = new VoznjaViewModel
                {
                    PodruzniceList = myListPodruznice,
                    VozaciList = _vozacService.GetVozaciSimple()
                };
                if (voznjaId != null)
                {
                    NamirniceDelivery.Data.Entities.Voznja voznja = _voznjaService.GetVoznja((int)voznjaId);
                    vm.PodruznicaKrajId = voznja.PodruznicaKrajId;
                    vm.PodruznicaPocetakId = voznja.PodruznicaPocetakId;
                    vm.VozacId = voznja.VozacId;
                }
                return View(vm);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [Authorize(Roles = "Menadzer")]
        public IActionResult Voznja(VoznjaViewModel model)
        {
            if (model.PodruznicaKrajId == model.PodruznicaPocetakId)
            {
                var podruznice = _podruznicaService.GetPodruznice();
                var myListPodruznice = new List<PodruznicaVM>();
                foreach (var p in podruznice) myListPodruznice.Add(new PodruznicaVM { Id = p.Id, Naziv = p.Naziv });
                model.PodruzniceList = myListPodruznice;
                model.VozaciList = _vozacService.GetVozaciSimple();
                return View(model);
            }
            _voznjaService.Voznja(model);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult DeleteVoznja(int voznjaId)
        {
            _voznjaService.DeleteVoznja(voznjaId);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Menadzer")]

        public async Task<IActionResult> GetExcelVoznje(CancellationToken cancellationToken)
        {
            await Task.Yield();
            var vozac = _vozacService.GetVozac(User.Identity.Name);
            var listVoznje = _voznjaService.GetVoznje();
            var list = new List<VoznjaExcel>();
            foreach (var item in listVoznje)
            {
                list.Add(new VoznjaExcel
                {
                    PocetakVoznje = item.VoznjaPocetak.ToString(),
                    KrajVoznje=item.VoznjaKraj.ToString(),
                    PodruznicaPocetak = item.PodruznicaPocetak.Naziv,
                    PodruznicaKraj = item.PodruznicaKraj.Naziv,
                    VozacIme = item.Vozac.Ime
                });
            }
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(list, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Voznje-{DateTime.Now.ToString("yyyy-MM-dd")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}
