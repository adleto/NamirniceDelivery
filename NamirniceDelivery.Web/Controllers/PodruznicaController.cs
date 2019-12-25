using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.ViewModels.Podruznica;

namespace NamirniceDelivery.Web.Controllers
{
    [Authorize(Roles="Kupac,AdministrativniRadnik")]
    public class PodruznicaController : Controller
    {
        private readonly IPodruznica _podruznicaService;
        private readonly IKupac _kupacService;
        private readonly INamirnicaPodruznica _namirnicaPodruznicaService;
        private readonly IAdministrativniRadnik _administrativniRadnikService;

        public PodruznicaController(IPodruznica podruznicaService, IKupac kupacService, INamirnicaPodruznica namirnicaPodruznicaService, IAdministrativniRadnik administrativniRadnikService)
        {
            _podruznicaService = podruznicaService;
            _kupacService = kupacService;
            _namirnicaPodruznicaService = namirnicaPodruznicaService;
            _administrativniRadnikService = administrativniRadnikService;
        }

        public IActionResult Index(int id, string returnUrl="")
        {
            var podruznica = _podruznicaService.GetPodruznica(id);
            var v = new IndexViewModel
            {
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
                if(kupac.OpcinaBoravkaId == podruznica.OpcinaId)
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
    }
}