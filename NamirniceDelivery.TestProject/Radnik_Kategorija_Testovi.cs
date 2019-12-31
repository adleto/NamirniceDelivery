using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.Controllers;
using NamirniceDelivery.Web.ViewModels.AdministrativniRadnik;
using System.Collections.Generic;

namespace NamirniceDelivery.TestProject
{
    [TestClass]
    public class Radnik_Kategorija_Testovi
    {
        Mock<IPopust> popustService;
        FakeSignInManager signInManager;
        Mock<IKategorija> kategorijaService;
        Mock<INamirnica> namirnicaService;
        Mock<INamirnicaPodruznica> namirnicaPodruznicaService;
        Mock<IAdministrativniRadnik> administrativniRadnikService;
        Mock<IAkcijeTransakcija> akcijeTransakcijaService;
        public Radnik_Kategorija_Testovi()
        {
            popustService = new Mock<IPopust>();
            signInManager = new FakeSignInManager();
            kategorijaService = new Mock<IKategorija>();
            namirnicaService = new Mock<INamirnica>();
            namirnicaPodruznicaService = new Mock<INamirnicaPodruznica>();
            administrativniRadnikService = new Mock<IAdministrativniRadnik>();
            akcijeTransakcijaService = new Mock<IAkcijeTransakcija>();
        }

        [TestMethod]
        public void Pregled_Kategorija_Test()
        {
            kategorijaService.Setup(k => k.GetKategorije()).Returns(new List<Kategorija> {
                new Kategorija()
                {
                    Id = 1,
                    Naziv = "Voće"
                },
                new Kategorija()
                {
                    Id = 2,
                    Naziv = "Povrće"
                }
            });
            AdministrativniRadnikController c = new AdministrativniRadnikController(signInManager, kategorijaService.Object, namirnicaService.Object, popustService.Object, namirnicaPodruznicaService.Object, administrativniRadnikService.Object, akcijeTransakcijaService.Object);
            ViewResult v = c.PregledKategorija() as ViewResult;
            PregledKategorijaViewModel p = v.Model as PregledKategorijaViewModel;
            Assert.AreEqual(p.KategorijaList.Count, 2);
        }
        [TestMethod]
        public void Kreiraj_Kategoriju_ModelNotNull()
        {
            AdministrativniRadnikController c = new AdministrativniRadnikController(signInManager, kategorijaService.Object, namirnicaService.Object, popustService.Object, namirnicaPodruznicaService.Object, administrativniRadnikService.Object, akcijeTransakcijaService.Object);
            ViewResult result = c.KreirajKategoriju() as ViewResult;
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Kreiraj_Kategoriju_NazivRedirecta()
        {
            AdministrativniRadnikController c = new AdministrativniRadnikController(signInManager, kategorijaService.Object, namirnicaService.Object, popustService.Object, namirnicaPodruznicaService.Object, administrativniRadnikService.Object, akcijeTransakcijaService.Object);
            kategorijaService.Setup(k => k.KreirajKategoriju(new Kategorija { }));
            var result = (RedirectToActionResult)c.KreirajKategoriju(new KreirajKategorijuViewModel { ReturnUrl = ""});
            Assert.AreEqual(nameof(c.PregledKategorija), result.ActionName);
        }
        [TestMethod]
        public void Edit_Kategorija_TacanId()
        {
            AdministrativniRadnikController c = new AdministrativniRadnikController(signInManager, kategorijaService.Object, namirnicaService.Object, popustService.Object, namirnicaPodruznicaService.Object, administrativniRadnikService.Object, akcijeTransakcijaService.Object);
            kategorijaService.Setup(k => k.GetKategorija(1)).Returns(new Kategorija { Id = 1 });
            var result = c.EditKategorija(1) as ViewResult;
            var model = (EditKategorijaViewModel)result.ViewData.Model;
            Assert.AreEqual(1, model.KategorijaId);
        }
    }
}
