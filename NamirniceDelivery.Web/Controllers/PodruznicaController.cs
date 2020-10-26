using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.ViewModels.Podruznica;
using cloudscribe.Pagination.Models;

namespace NamirniceDelivery.Web.Controllers
{
    public class PodruznicaController : Controller
    {
        private readonly IPodruznica _podruznicaService;
        private readonly IKupac _kupacService;
        private readonly INamirnicaPodruznica _namirnicaPodruznicaService;
        private readonly IAdministrativniRadnik _administrativniRadnikService;
        private readonly IApplicationUser _applicationUserService;
        private readonly IOpcina _opcinaService;

        public PodruznicaController(IPodruznica podruznicaService, IKupac kupacService, INamirnicaPodruznica namirnicaPodruznicaService, IAdministrativniRadnik administrativniRadnikService, IApplicationUser applicationUserService, IOpcina opcinaService)
        {
            _podruznicaService = podruznicaService;
            _kupacService = kupacService;
            _namirnicaPodruznicaService = namirnicaPodruznicaService;
            _administrativniRadnikService = administrativniRadnikService;
            _applicationUserService = applicationUserService;
            _opcinaService = opcinaService;
        }


        //public IActionResult Index(int pageNumber = 1, int pageSize = 3)
        //{
        //    var result = new PagedResult<Podruznica>();

        //    int ExcludeRecords = (pageSize * pageNumber) - pageSize;

        //    var query = _podruznicaService.GetPodruznice().Skip(ExcludeRecords).Take(pageSize);

        //    result = new PagedResult<Podruznica>
        //    {
        //        //Data = query.AsNoTracking().ToList(),
        //        TotalItems = _podruznicaService.GetPodruznice().Count(),
        //        PageNumber = pageNumber,
        //        PageSize = pageSize
        //    };
        //    return View(result);

        //}


        [Authorize(Roles = "Kupac,AdministrativniRadnik,Menadzer")]
        public IActionResult Index(int id, string returnUrl = "")
        {
            var podruznica = _podruznicaService.GetPodruznica(id);
            var v = new IndexViewModel
            {
                PodruznicaId = podruznica.Id,
                Adresa = podruznica.Adresa,
                Naziv = podruznica.Naziv,
                Opcina = podruznica.Opcina,
                Opis = podruznica.Opis,
                ReturnUrl = returnUrl,
                MozeKupovati_ZaKupca = false,
                IsFavourite_ZaKupca = false,
                NamirnicaList = _namirnicaPodruznicaService.GetNamirnicePodruznica(podruznica.Id)
            };
            if (User.IsInRole("Kupac"))
            {
                var kupac = _kupacService.GetKupac(User.Identity.Name);
                if (kupac.OpcinaBoravkaId == podruznica.OpcinaId)
                {
                    v.MozeKupovati_ZaKupca = true;
                }
                if (KonvertujSpremljeneUPodruznice(_kupacService.GetSpremljenePodruznice(kupac.Id)).Contains(podruznica))
                {
                    v.IsFavourite_ZaKupca = true;
                }
                v.SpremljeneNamirniceList = KonvertujSpremljeneUNamirnice(_kupacService.GetSpremljeneNamirnice(kupac.Id));
            }
            return View(v);
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


        // -- FROM HERE (down) ZA MENADZERA
        [Authorize(Roles = "Menadzer")]
        public IActionResult PregledPodruznica()
        {
            var model = new PregledPodruznicaVM
            {
                PodruznicaList = _podruznicaService.GetPodruznice()
            };
            return View(model);
        }
        [Authorize(Roles = "Menadzer")]
        public IActionResult Podruznica(int podruznicaId = 0)
        {

            var model = new PodruznicaVM
            {
                OpcinaList = _opcinaService.GetOpcine(),
                PodruznicaId = podruznicaId
            };
            if (podruznicaId != 0)
            {
                var p = _podruznicaService.GetPodruznica(podruznicaId);
                model.Naziv = p.Naziv;
                model.Adresa = p.Adresa;
                model.OpcinaId = p.OpcinaId;
                model.Opis = p.Opis;
                model.PodruznicaId = p.Id;
            }
            return View(model);
        }
        [Authorize(Roles = "Menadzer")]
        [HttpPost]
        public IActionResult Podruznica(PodruznicaVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.PodruznicaId != 0)
                {
                    _podruznicaService.EditPodruznica(new Podruznica
                    {
                        Adresa = model.Adresa,
                        Naziv = model.Naziv,
                        Opis = model.Opis,
                        OpcinaId = model.OpcinaId,
                        Id = model.PodruznicaId
                    });
                }
                else
                {
                    _podruznicaService.KreirajPodruznicu(new Podruznica
                    {
                        Adresa = model.Adresa,
                        Naziv = model.Naziv,
                        Opis = model.Opis,
                        OpcinaId = model.OpcinaId
                    });
                }
                return RedirectToAction(nameof(PregledPodruznica));
            }
            model.OpcinaList = _opcinaService.GetOpcine();
            return View(model);
        }
        [Authorize(Roles = "Menadzer")]
        public IActionResult ObrisiPodruznica(int podruznicaId = 0)
        {
            _podruznicaService.ObrisiPodruznicu(podruznicaId);
            return RedirectToAction(nameof(PregledPodruznica));
        }
    }
}