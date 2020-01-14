using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NamirniceDelivery.Web.Controllers
{
    [Authorize(Roles ="Menadzer")]
    public class MenadzerController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("PregledPodruznica","Podruznica");
        }
    }
}