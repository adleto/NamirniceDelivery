using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.Controllers;
using NamirniceDelivery.Web.Hubs;
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
        Mock<IHubContext<MyHub>> _hubContext;
        public Radnik_Kategorija_Testovi()
        {
            popustService = new Mock<IPopust>();
            signInManager = new FakeSignInManager();
            kategorijaService = new Mock<IKategorija>();
            namirnicaService = new Mock<INamirnica>();
            namirnicaPodruznicaService = new Mock<INamirnicaPodruznica>();
            administrativniRadnikService = new Mock<IAdministrativniRadnik>();
            akcijeTransakcijaService = new Mock<IAkcijeTransakcija>();
            _hubContext = new Mock<IHubContext<MyHub>>();
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
            AdministrativniRadnikController c = new AdministrativniRadnikController(signInManager, kategorijaService.Object, namirnicaService.Object, popustService.Object, namirnicaPodruznicaService.Object, administrativniRadnikService.Object, akcijeTransakcijaService.Object, _hubContext.Object);
            PartialViewResult v = c.PregledKategorijaGetData() as PartialViewResult;
            KategorijaListViewModel p = v.Model as KategorijaListViewModel;
            Assert.AreEqual(p.KategorijaList.Count, 2);
            c.Dispose();
        }
        //[TestMethod]
        //public void Kreiraj_Kategoriju_ModelNotNull()
        //{
        //    AdministrativniRadnikController c = new AdministrativniRadnikController(signInManager, kategorijaService.Object, namirnicaService.Object, popustService.Object, namirnicaPodruznicaService.Object, administrativniRadnikService.Object, akcijeTransakcijaService.Object, _hubContext.Object);
        //    ViewResult result = c.KategorijaAdd(new KategorijaPartialViewModel { }).Result as ViewResult;
        //    Assert.IsNotNull(result);
        //    c.Dispose();
        //}
        //[TestMethod]
        //public void Edit_Kategorija_TacanId()
        //{
        //    AdministrativniRadnikController c = new AdministrativniRadnikController(signInManager, kategorijaService.Object, namirnicaService.Object, popustService.Object, namirnicaPodruznicaService.Object, administrativniRadnikService.Object, akcijeTransakcijaService.Object, _hubContext.Object);
        //    kategorijaService.Setup(k => k.GetKategorija(1)).Returns(new Kategorija { Id = 1 });
        //    var result = c.KategorijaEdit(new KategorijaPartialViewModel { KategorijaId = 1 }).Result as ViewResult;
        //    var model = (KategorijaPartialViewModel)result.ViewData.Model;
        //    Assert.AreEqual(1, model.KategorijaId);
        //}
    }
}
