using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.Models;
using NamirniceDelivery.Web.ViewModels.Home;

namespace NamirniceDelivery.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static SignInManager<ApplicationUser> _signInManager;
        private readonly INamirnicaPodruznica _namirnicaPodruznicaService;
        private readonly IPodruznica _podruznicaService;

        public HomeController(ILogger<HomeController> logger, SignInManager<ApplicationUser> signInManager, INamirnicaPodruznica namirnicaPodruznicaService, IPodruznica podruznicaService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _podruznicaService = podruznicaService;
            _namirnicaPodruznicaService = namirnicaPodruznicaService;
        }

        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (User.IsInRole("AdministrativniRadnik"))
                {
                    return RedirectToAction("Index", "AdministrativniRadnik");
                }
                else if (User.IsInRole("Kupac"))
                {
                    return RedirectToAction("Index", "Kupac");
                }
            }
            return View(new IndexViewModel { 
                NamirnicaList = _namirnicaPodruznicaService.GetNamirnicePodruznica(),
                PodruznicaList = _podruznicaService.GetPodruznice()
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
