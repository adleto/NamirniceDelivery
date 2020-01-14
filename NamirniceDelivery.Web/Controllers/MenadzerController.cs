using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NamirniceDelivery.Data.Entities;

namespace NamirniceDelivery.Web.Controllers
{
    
    public class MenadzerController : Controller
    {
        private static SignInManager<ApplicationUser> _signInManager;

        public MenadzerController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> DemoLogin()
        {
            await _signInManager.PasswordSignInAsync("menadzerMain", "password", false, lockoutOnFailure: true);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Menadzer")]
        public IActionResult Index()
        {
            return RedirectToAction("PregledPodruznica","Podruznica");
        }
    }
}