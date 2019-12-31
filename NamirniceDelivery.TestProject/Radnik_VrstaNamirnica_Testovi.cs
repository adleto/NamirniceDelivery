using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.Controllers;
using NamirniceDelivery.Web.ViewModels.AdministrativniRadnik;
using System;
using System.Collections.Generic;
using System.Text;

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

        public Radnik_VrstaNamirnica_Testovi()
        {
            popustService = new Mock<IPopust>();
            signInManager = new FakeSignInManager();
            kategorijaService = new Mock<IKategorija>();
            namirnicaService = new Mock<INamirnica>();
            namirnicaPodruznicaService = new Mock<INamirnicaPodruznica>();
            administrativniRadnikService = new Mock<IAdministrativniRadnik>();
            akcijeTransakcijaService = new Mock<IAkcijeTransakcija>();
            c = new AdministrativniRadnikController(signInManager, kategorijaService.Object, namirnicaService.Object, popustService.Object, namirnicaPodruznicaService.Object, administrativniRadnikService.Object, akcijeTransakcijaService.Object);
        }
        [TestMethod]
        public void Radnik_KreiraVrstuNamirnica_ModelNotNull()
        {
            ViewResult result = (ViewResult)c.KreirajNamirnica();
            Assert.IsNotNull(result.Model);
        }
        [TestMethod]
        public void Radnik_KreiraVrstuNamirnica_RedirectToUrl()
        {
            RedirectResult redirect = (RedirectResult)c.KreirajNamirnica(new KreirajNamirnicaViewModel { ReturnUrl = "url" });
            Assert.AreEqual("url",redirect.Url);
        }
        [TestMethod]
        public void Radnik_PregledaNamirnice_PostojeNamirniceUModelu()
        {
            kategorijaService.Setup(k => k.GetKategorije()).Returns(new List<Kategorija> { });
            namirnicaService.Setup(n => n.GetNamirnicePoKategorijama(new Kategorija { })).Returns(new List<Namirnica> { });
            namirnicaService.Setup(n => n.GetIsDeletable(new List<Namirnica> { })).Returns(new List<bool> { });
            namirnicaService.Setup(n => n.GetNamirnice()).Returns(new List<Namirnica> { new Namirnica { } });

            ViewResult v = c.PregledNamirnica() as ViewResult;
            PregledNamirnicaViewModel p = v.Model as PregledNamirnicaViewModel;

            Assert.AreEqual(p.NamirnicaList.Count, 1);
        }
        [TestMethod]
        public void Radnik_EditNamirnica_TacanNazivNamirnice()
        {
            kategorijaService.Setup(k => k.GetKategorija(1)).Returns(new Kategorija { });
            namirnicaService.Setup(n => n.GetNamirnica(1)).Returns(new Namirnica { Id = 1, Naziv = "NazivNamirnica", Kategorija = new Kategorija { Id = 1 } });
            ViewResult v = c.EditNamirnica(1) as ViewResult;
            EditNamirnicaViewModel model = v.Model as EditNamirnicaViewModel;
            Assert.AreEqual("NazivNamirnica",model.Naziv);
        }
    }
}
