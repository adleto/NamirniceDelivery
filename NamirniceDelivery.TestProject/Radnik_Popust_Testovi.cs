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
    public class Radnik_Popust_Testovi
    {
        Mock<IPopust> popustService;
        FakeSignInManager signInManager;
        Mock<IKategorija> kategorijaService;
        Mock<INamirnica> namirnicaService;
        Mock<INamirnicaPodruznica> namirnicaPodruznicaService;
        Mock<IAdministrativniRadnik> administrativniRadnikService;
        Mock<IAkcijeTransakcija> akcijeTransakcijaService;
        AdministrativniRadnikController c;
        public Radnik_Popust_Testovi()
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
    }
}
