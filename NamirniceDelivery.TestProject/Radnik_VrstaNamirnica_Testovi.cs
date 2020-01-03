using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.Controllers;
using NamirniceDelivery.Web.Hubs;
using NamirniceDelivery.Web.ViewModels.AdministrativniRadnik;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NamirniceDelivery.TestProject
{
    [TestClass]
    public class Radnik_VrstaNamirnica_Testovi
    {
        Mock<IPopust> popustService;
        FakeSignInManager signInManager;
        Mock<IKategorija> kategorijaService;
        Mock<INamirnica> namirnicaService;
        Mock<INamirnicaPodruznica> namirnicaPodruznicaService;
        Mock<IAdministrativniRadnik> administrativniRadnikService;
        Mock<IAkcijeTransakcija> akcijeTransakcijaService;
        AdministrativniRadnikController c;
        Mock<IHubContext<MyHub>> hubContext;

        public Radnik_VrstaNamirnica_Testovi()
        {
            popustService = new Mock<IPopust>();
            signInManager = new FakeSignInManager();
            kategorijaService = new Mock<IKategorija>();
            namirnicaService = new Mock<INamirnica>();
            namirnicaPodruznicaService = new Mock<INamirnicaPodruznica>();
            administrativniRadnikService = new Mock<IAdministrativniRadnik>();
            akcijeTransakcijaService = new Mock<IAkcijeTransakcija>();
            hubContext = new Mock<IHubContext<MyHub>>();
            c = new AdministrativniRadnikController(signInManager, kategorijaService.Object, namirnicaService.Object, popustService.Object, namirnicaPodruznicaService.Object, administrativniRadnikService.Object, akcijeTransakcijaService.Object, hubContext.Object);
        }
        //[TestMethod]
        //public void Radnik_KreiraVrstuNamirnica_NotNull()
        //{
        //    hubContext.Setup(h => h.Clients.All.SendAsync(It.IsAny<string>(),It.IsAny<CancellationToken>()));
        //    ObjectResult result = (ObjectResult)c.NamirnicaAdd(new NamirnicaPartialViewModel { }).Result;
        //    Assert.IsNotNull(result);
        //}
        [TestMethod]
        public void Radnik_PregledaNamirnice_PostojeNamirniceUModelu()
        {
            kategorijaService.Setup(k => k.GetKategorije()).Returns(new List<Kategorija> { new Kategorija { } });
            namirnicaService.Setup(n => n.GetNamirnicePoKategorijama(new Kategorija { })).Returns(new List<Namirnica> { });
            namirnicaService.Setup(n => n.GetIsDeletable(new List<Namirnica> { })).Returns(new List<bool> { });
            namirnicaService.Setup(n => n.GetNamirnice()).Returns(new List<Namirnica> { new Namirnica { } });

            ViewResult v = c.PregledNamirnica() as ViewResult;
            PregledNamirnicaViewModel p = v.Model as PregledNamirnicaViewModel;

            Assert.AreEqual(p.KategorijaList.Count, 1);
        }
        //[TestMethod]
        //public void Radnik_EditNamirnica_TacanNazivNamirnice()
        //{
        //    kategorijaService.Setup(k => k.GetKategorija(1)).Returns(new Kategorija { });
        //    namirnicaService.Setup(n => n.GetNamirnica(1)).Returns(new Namirnica { Id = 1, Naziv = "NazivNamirnica", Kategorija = new Kategorija { Id = 1 } });
        //    ViewResult v = c.NamirnicaEdit(new NamirnicaPartialViewModel { NamirnicaId = 1}).Result as ViewResult;
        //    NamirnicaPartialViewModel model = v.Model as NamirnicaPartialViewModel;
        //    Assert.AreEqual("NazivNamirnica",model.Naziv);
        //}
    }
}
