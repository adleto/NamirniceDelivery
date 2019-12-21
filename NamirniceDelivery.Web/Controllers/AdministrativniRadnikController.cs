using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.ViewModels.AdministrativniRadnik;

namespace NamirniceDelivery.Web.Controllers
{
    public class AdministrativniRadnikController : Controller
    {
        private static SignInManager<ApplicationUser> _signInManager;
        private readonly IKategorija _kategorijaService;

        public AdministrativniRadnikController(SignInManager<ApplicationUser> signInManager, IKategorija kategorijaService)
        {
            _signInManager = signInManager;
            _kategorijaService = kategorijaService;
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
        public IActionResult KreirajKategoriju(string returnUrl = "")
        {
            return View(new KreirajKategorijuViewModel
            {
                ReturnUrl = returnUrl
            });
        }
        [HttpPost]
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult KreirajKategoriju(KreirajKategorijuViewModel model)
        {
            if (ModelState.IsValid)
            {
                _kategorijaService.KreirajKategoriju(new Kategorija
                {
                    Naziv = model.Naziv
                });
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction(nameof(PregledKategorija));
            }
            return View(model);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult PregledKategorija(string returnUrl = "")
        {
            return View(new PregledKategorijaViewModel
            {
                ReturnUrl = returnUrl,
                KategorijaList = _kategorijaService.GetKategorije()
            });
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult EditKategorija(int kategorijaId, string returnUrl = "")
        {
            var kategorija = _kategorijaService.GetKategorija(kategorijaId);
            return View(new EditKategorijaViewModel
            {
                KategorijaId = kategorija.Id,
                Naziv = kategorija.Naziv,
                ReturnUrl = returnUrl
            });
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        [HttpPost]
        public IActionResult EditKategorija(EditKategorijaViewModel model)
        {
            if (ModelState.IsValid)
            {
                _kategorijaService.EditKategorija(new Kategorija
                {
                    Id = model.KategorijaId,
                    Naziv = model.Naziv
                });
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}