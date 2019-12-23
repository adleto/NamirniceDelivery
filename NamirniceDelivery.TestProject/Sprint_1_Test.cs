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
    public class Sprint_1_Test
    {
        Mock<IPopust> popustService;
        FakeSignInManager signInManager;
        Mock<IKategorija> kategorijaService;
        Mock<INamirnica> namirnicaService;

        public Sprint_1_Test()
        {
            popustService = new Mock<IPopust>();
            signInManager = new FakeSignInManager();
            kategorijaService = new Mock<IKategorija>();
            namirnicaService = new Mock<INamirnica>();
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
            AdministrativniRadnikController c = new AdministrativniRadnikController(signInManager,kategorijaService.Object, namirnicaService.Object, popustService.Object);
            ViewResult v = c.PregledKategorija() as ViewResult;
            PregledKategorijaViewModel p = v.Model as PregledKategorijaViewModel;
            Assert.AreEqual(p.KategorijaList.Count, 2);
        }
    }
}
