using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NamirniceDelivery.Data.Entities;

namespace NamirniceDelivery.Web.Controllers
{
    public class AdministrativniRadnikController : Controller
    {
        private static SignInManager<ApplicationUser> _signInManager;

        public AdministrativniRadnikController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> DemoLogin()
        {
            await _signInManager.PasswordSignInAsync("testRadnik", "password", false, lockoutOnFailure: true);
            return RedirectToAction(nameof(Index));
        }
    }
}