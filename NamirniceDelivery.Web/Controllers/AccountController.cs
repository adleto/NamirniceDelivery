using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.ViewModels.Account;

namespace NamirniceDelivery.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IOpcina _opcinaService;
        private readonly IKupac _kupacService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, IEmailSender emailSender, IOpcina opcinaService, IKupac kupacService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _opcinaService = opcinaService;
            _kupacService = kupacService;
        }

        public async Task<IActionResult> Login(string returnUrl = null)
        {
            var model = new LoginVM
            {
                ErrorMessage = ""
            };
            if (!string.IsNullOrEmpty(model.ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, model.ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect("/Home/Index");
                }
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                //}
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Nalog zaključan.");
                    return RedirectToPage("./Lockout"); //dodaj lockout
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Pogrešan username ili lozinka.");
                    return View(model);
                }
            }
            // If we got this far, something failed, redisplay form
            return RedirectToAction(nameof(Login));
        }
        public IActionResult Register(string returnUrl = null)
        {
            var model = new RegisterVM{
                ReturnUrl = returnUrl,
                OpcinaList = _opcinaService.GetOpcine(),
                ErrorMessage = ""
            };

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/");
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new Kupac { 
                    UserName = model.Username, 
                    Email = model.Email,
                    Adresa = model.Adresa,
                    Ime = model.Ime,
                    OpcinaBoravkaId = model.OpcinaBoravkaId,
                    OpcinaRodjenjaId = model.OpcinaRodjenjaId,
                    RejtingKupac = 0,
                    Prezime = model.Prezime
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = model.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect("/Home/Index");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction(nameof(Register));
        }
    }
}