using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.Hubs;
using NamirniceDelivery.Web.Models;
using NamirniceDelivery.Web.ViewModels.AdministrativniRadnik;
using NamirniceDelivery.Web.ViewModels.Shared;
using OfficeOpenXml;

namespace NamirniceDelivery.Web.Controllers
{
    public class AdministrativniRadnikController : Controller
    {
        private static SignInManager<ApplicationUser> _signInManager;
        private readonly IKategorija _kategorijaService;
        private readonly INamirnica _namirnicaService;
        private readonly IPopust _popustService;
        private readonly INamirnicaPodruznica _namirnicaPodruznicaService;
        private readonly IAdministrativniRadnik _administrativniRadnikService;
        private readonly IAkcijeTransakcija _akcijeTransakcijaService;
        private readonly IHubContext<MyHub> _hubContext;

        public AdministrativniRadnikController(SignInManager<ApplicationUser> signInManager, IKategorija kategorijaService, INamirnica namirnicaService, IPopust popustService, INamirnicaPodruznica namirnicaPodruznicaService, IAdministrativniRadnik administrativniRadnikService, IAkcijeTransakcija akcijeTransakcijaService, IHubContext<MyHub> hubContext)
        {
            _signInManager = signInManager;
            _kategorijaService = kategorijaService;
            _namirnicaService = namirnicaService;
            _popustService = popustService;
            _namirnicaPodruznicaService = namirnicaPodruznicaService;
            _administrativniRadnikService = administrativniRadnikService;
            _akcijeTransakcijaService = akcijeTransakcijaService;
            _hubContext = hubContext;
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> DemoLogin()
        {
            await _signInManager.PasswordSignInAsync("testRadnik", "password", false, lockoutOnFailure: true);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult PregledKategorijaGetData()
        {
            var kategorijaList = _kategorijaService.GetKategorije();
            return PartialView("_KategorijaListPartialView",new KategorijaListViewModel
            {
                KategorijaList = kategorijaList,
                Deletable = _kategorijaService.GetIsDeletable(kategorijaList)
            });
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult PregledKategorija(string returnUrl = "")
        {
            return View(new PregledViewModel
            {
                ReturnUrl = returnUrl
            });
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult Kategorija(int id = 0)
        {
            var model = new KategorijaPartialViewModel { KategorijaId = id};
            if (id != 0)
            {
                model.Naziv = _kategorijaService.GetKategorija(id).Naziv;
            }
            return PartialView("_KategorijaPartialView", model);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        [HttpPost]
        public async Task<IActionResult> KategorijaAdd(KategorijaPartialViewModel model)
        {
            if (ModelState.IsValid)
            {
                _kategorijaService.KreirajKategoriju(new Kategorija
                {
                    Naziv = model.Naziv
                });
                await _hubContext.Clients.All.SendAsync("Repopulate");
                return Ok("Ok");
            }
            return PartialView("_KategorijaPartialView", model);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        [HttpPost]
        public async Task<IActionResult> KategorijaEdit(KategorijaPartialViewModel model)
        {
            if (ModelState.IsValid)
            {
                _kategorijaService.EditKategorija(new Kategorija
                {
                    Id = model.KategorijaId,
                    Naziv = model.Naziv
                });
                await _hubContext.Clients.All.SendAsync("Repopulate");
                return Ok("Ok");
            }
            return PartialView("_KategorijaPartialView", model);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public async Task<IActionResult> UkloniKategoriju(int id)
        {
            if (!_namirnicaService.GetNamirnicePoKategorijama(_kategorijaService.GetKategorija(id)).Any())
            {
                _kategorijaService.UkloniKategorija(id);
                await _hubContext.Clients.All.SendAsync("Repopulate");
                return Ok("Ok");
            }
            return Ok("Failed to delete");
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult KreirajNamirnica(string returnUrl = "")
        {
            return View(new KreirajNamirnicaViewModel
            {
                ReturnUrl = returnUrl,
                KategorijaList = _kategorijaService.GetKategorije()
            });
        }
        [HttpPost]
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult KreirajNamirnica(KreirajNamirnicaViewModel model)
        {
            if (ModelState.IsValid)
            {
                _namirnicaService.KreirajNamirnica(new Namirnica
                {
                    Naziv = model.Naziv,
                    KategorijaId = model.KategorijaId
                });
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction(nameof(PregledKategorija));
            }
            model.KategorijaList = _kategorijaService.GetKategorije();
            return View(model);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult PregledNamirnica(string returnUrl = "", int kategorijaId=0)
        {
            var v = new PregledNamirnicaViewModel
            {
                ReturnUrl = returnUrl,
                KategorijaList = _kategorijaService.GetKategorije()
            };
            if (kategorijaId!=0)
            {
                v.NamirnicaList = _namirnicaService.GetNamirnicePoKategorijama(_kategorijaService.GetKategorija(kategorijaId));
                v.KategorijaId = kategorijaId;
            }
            else
            {
                v.NamirnicaList = _namirnicaService.GetNamirnice();
                v.KategorijaId = 0;
            }
            v.Deletable = _namirnicaService.GetIsDeletable(v.NamirnicaList);
            return View(v);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult EditNamirnica(int namirnicaId, string returnUrl = "")
        {
            var namirnica = _namirnicaService.GetNamirnica(namirnicaId);
            return View(new EditNamirnicaViewModel
            {
                KategorijaId = namirnica.Kategorija.Id,
                Naziv = namirnica.Naziv,
                ReturnUrl = returnUrl,
                NamirnicaId = namirnica.Id,
                KategorijaList = _kategorijaService.GetKategorije()
            });
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        [HttpPost]
        public IActionResult EditNamirnica(EditNamirnicaViewModel model)
        {
            if (ModelState.IsValid)
            {
                _namirnicaService.EditNamirnica(new Namirnica
                {
                    Id = model.NamirnicaId,
                    Naziv = model.Naziv,
                    KategorijaId = model.KategorijaId
                });
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction(nameof(PregledNamirnica));
            }
            model.KategorijaList = _kategorijaService.GetKategorije();
            return View(model);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult UkloniNamirnicu(int namirnicaId, string returnUrl = "")
        {
            if (!_namirnicaPodruznicaService.GetNamirnicePodruznicaVrsta(_namirnicaService.GetNamirnica(namirnicaId)).Any())
            {
                _namirnicaService.UkloniNamirnica(namirnicaId);
            }
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(PregledNamirnica));
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult KreirajPopust(string returnUrl = "")
        {
            return View(new KreirajPopustViewModel
            {
                ReturnUrl = returnUrl
            });
        }
        [HttpPost]
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult KreirajPopust(KreirajPopustViewModel model)
        {
            if (ModelState.IsValid)
            {
                _popustService.KreirajPopust(new Popust
                {
                    Iznos = (model.Iznos / 100) ?? 1,
                    Opis = model.Opis
                });
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction(nameof(PregledPopust));
            }
            return View(model);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult PregledPopust(string returnUrl = "")
        {
            var v = new PregledPopustViewModel
            {
                ReturnUrl = returnUrl,
                PopustList = _popustService.GetPopusti()
            };
            v.Deletable = _popustService.GetIsDeletable(v.PopustList);
            return View(v);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult EditPopust(int popustId, string returnUrl = "")
        {
            var popust = _popustService.GetPopust(popustId);
            return View(new EditPopustViewModel
            {
                Iznos = popust.Iznos*100,
                Opis = popust.Opis,
                ReturnUrl = returnUrl,
            });
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        [HttpPost]
        public IActionResult EditPopust(EditPopustViewModel model)
        {
            if (ModelState.IsValid)
            {
                _popustService.EditPopust(new Popust
                {
                    Id = model.PopustId,
                    Opis = model.Opis,
                    Iznos = (model.Iznos/100)??1
                });
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction(nameof(PregledNamirnica));
            }
            return View(model);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult UkloniPopust(int popustId, string returnUrl = "")
        {
            if (!_namirnicaPodruznicaService.GetNamirnicePodruznicaPopust(_popustService.GetPopust(popustId)).Any())
            {
                _popustService.UkloniPopust(popustId);
            }
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(PregledPopust));
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult DodajNamirnicu(string returnUrl = "")
        {
            return View(new DodajNamirnicuViewModel
            {
                PopustList = _popustService.GetPopusti(),
                NamirnicaList = _namirnicaService.GetNamirnice(),
                ReturnUrl = returnUrl
            });
        }
        [HttpPost]
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult DodajNamirnicu(DodajNamirnicuViewModel model)
        {
            if (ModelState.IsValid)
            {
                var namirnica = new NamirnicaPodruznica
                {
                    Aktivna = true,
                    Cijena = model.Cijena ?? 1,
                    KolicinaNaStanju = model.KolicinaNaStanju ?? 1,
                    NamirnicaId = model.NamirnicaId,
                    PodruznicaId = _administrativniRadnikService.GetPodruznicaIdOdRadnika(User.Identity.Name)
                };
                if (model.PopustId != 0)
                {
                    namirnica.PopustId = model.PopustId;
                }
                _namirnicaPodruznicaService.DodajNamirnicu(namirnica);
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction(nameof(PregledNamirnicaPodruznica));
            }
            model.PopustList = _popustService.GetPopusti();
            model.NamirnicaList = _namirnicaService.GetNamirnice();
            return View(model);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult PregledNamirnicaPodruznica(string returnUrl = "", int kategorijaId = 0)
        {
            int podruznicaId = _administrativniRadnikService.GetPodruznicaIdOdRadnika(User.Identity.Name);
            var v = new PregledNamirnicaPodruznicaViewModel
            {
                ReturnUrl = returnUrl,
                KategorijaList = _kategorijaService.GetKategorije()
            };
            if (kategorijaId != 0)
            {
                v.NamirnicaList = _namirnicaPodruznicaService.GetNamirnicePodruznicaKategorija(_kategorijaService.GetKategorija(kategorijaId), podruznicaId);
                v.KategorijaId = kategorijaId;
            }
            else
            {
                v.NamirnicaList = _namirnicaPodruznicaService.GetNamirnicePodruznica(podruznicaId);
                v.KategorijaId = 0;
            }
            return View(v);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult UkloniNamirnicaPodruznica(int namirnicaPodruznicaId, string returnUrl="")
        {
            if (_namirnicaPodruznicaService.GetNamirnicaPodruznica(namirnicaPodruznicaId).PodruznicaId == _administrativniRadnikService.GetPodruznicaIdOdRadnika(User.Identity.Name))
            {
                _namirnicaPodruznicaService.UkloniNamirnicaPodruznica(namirnicaPodruznicaId);
            }
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(PregledNamirnicaPodruznica));
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult NamirnicaToogleStatus(int namirnicaPodruznicaId, string returnUrl = "")
        {
            if (_namirnicaPodruznicaService.GetNamirnicaPodruznica(namirnicaPodruznicaId).PodruznicaId == _administrativniRadnikService.GetPodruznicaIdOdRadnika(User.Identity.Name))
            {
                _namirnicaPodruznicaService.ToogleStatusNamirnicaPodruznica(namirnicaPodruznicaId);
            }
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(PregledNamirnicaPodruznica));
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult EditNamirnicaPodruznica(int namirnicaPodruznicaId, string returnUrl = "")
        {
            var namirnica = _namirnicaPodruznicaService.GetNamirnicaPodruznica(namirnicaPodruznicaId);
            var model = new EditNamirnicaPodruznicaViewModel
            {
                Aktivna = namirnica.Aktivna,
                Cijena = namirnica.Cijena,
                KolicinaNaStanju = namirnica.KolicinaNaStanju,
                Naziv = namirnica.Namirnica.Naziv,
                NamirnicaPodruznicaId = namirnica.Id,
                PopustList = _popustService.GetPopusti()
            };
            if (namirnica.Popust != null)
            {
                model.PopustId = namirnica.PopustId??1;
            }
            return View(model);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        [HttpPost]
        public IActionResult EditNamirnicaPodruznica(EditNamirnicaPodruznicaViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_namirnicaPodruznicaService.GetNamirnicaPodruznica(model.NamirnicaPodruznicaId).PodruznicaId == _administrativniRadnikService.GetPodruznicaIdOdRadnika(User.Identity.Name))
                {
                    NamirnicaPodruznica namirnica = new NamirnicaPodruznica
                    {
                        Id = model.NamirnicaPodruznicaId,
                        Aktivna = model.Aktivna,
                        Cijena = model.Cijena ?? 1,
                        KolicinaNaStanju = model.KolicinaNaStanju
                    };
                    if (model.PopustId != 0)
                    {
                        namirnica.PopustId = model.PopustId;
                    }
                    _namirnicaPodruznicaService.EditNamirnicaPodruznica(namirnica);
                }
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction(nameof(PregledNamirnicaPodruznica));
            }
            model.PopustList = _popustService.GetPopusti();
            return View(model);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult PrihvatiNarudzbu(int transakcijaId, string returnUrl="")
        {
            var radnik = _administrativniRadnikService.GetRadnik(User.Identity.Name);
            _akcijeTransakcijaService.OdobriTranskaciju(transakcijaId, radnik);
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("DostaveUToku", "Transakcija");
            }
            return Redirect(returnUrl);
        }
        [Authorize(Roles="AdministrativniRadnik")]
        public async Task<IActionResult> GetExcelNamirnicePodruznica(CancellationToken cancellationToken)  
        {  
            await Task.Yield();
            var radnik = _administrativniRadnikService.GetRadnik(User.Identity.Name);
            var listNamirnice = _namirnicaPodruznicaService.GetNamirnicePodruznica(radnik.PodruznicaId ?? 0);
            var list = new List<NamirnicaExcel>();
            foreach (var item in listNamirnice)
            {
                list.Add(new NamirnicaExcel
                {
                    Broj = (listNamirnice.IndexOf(item) + 1).ToString(),
                    Cijena = item.Cijena.ToString("F") + "KM",
                    CijenaSaPopustom = item.CijenaSaPopustom.ToString("F") + "KM",
                    KolicinaNaStanju = item.KolicinaNaStanju.ToString(),
                    Namirnica = item.Namirnica.Naziv
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
            string excelName = $"Namirnice-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";  
  
            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);  
        }
    }
}