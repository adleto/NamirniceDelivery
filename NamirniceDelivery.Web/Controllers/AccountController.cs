using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.Helper;
using NamirniceDelivery.Web.ViewModels.Account;
using Nexmo.Api;

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
        private readonly IApplicationUser _applicationUserService;
        private readonly UrlEncoder _urlEncoder;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, IEmailSender emailSender, IOpcina opcinaService, IKupac kupacService, IApplicationUser applicationUserService, UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _opcinaService = opcinaService;
            _kupacService = kupacService;
            _applicationUserService = applicationUserService;
            _urlEncoder = urlEncoder;
        }
        [AnonymousOnly]
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
        [AnonymousOnly]
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
                    var client = new Client(creds: new Nexmo.Api.Request.Credentials
                    {
                        ApiKey = "c5a955d2",
                        ApiSecret = "NDufdSI857gZiXvy"
                    });
                    //var results = client.SMS.Send(request: new SMS.SMSRequest
                    //{
                    //    from = "Vonage APIs",
                    //    to = "38763671092",
                    //    text = "Uspješno ste se prijavili na NamirniceDelivery. <3"
                    //});
                    _applicationUserService.SetLogedInTimeStamp(_applicationUserService.GetUser(model.Username));

                    _logger.LogInformation("User logged in.");
                    return LocalRedirect("/Home/Index");
                }

                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction("LoginWith2fa", new { rememberMe = model.RememberMe });
                }

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

        public IActionResult LoginWith2fa(bool rememberMe)
        {
            var model = new LoginWith2faVM
            {
                RememberMe = rememberMe
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var korisnik = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (korisnik == null)
            {
                throw new InvalidOperationException($"Greška pri 2FA.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, model.RememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                _applicationUserService.SetLogedInTimeStamp(_applicationUserService.GetUser(korisnik.UserName));
                return RedirectToAction("Index","Home");
            }
            else if (result.IsLockedOut)
            {
                //Korisnik locked out
                return RedirectToAction("Index", "Home");
            }
            else
            {
                model.StatusMessage = "Pogrešan kod unesen.";
                return View(model);
            }
        }


        [AnonymousOnly]
        public IActionResult Register(string returnUrl = null)
        {
            var model = new RegisterVM
            {
                ReturnUrl = returnUrl,
                OpcinaList = _opcinaService.GetOpcine(),
                ErrorMessage = ""
            };

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return View(model);
        }
        [AnonymousOnly]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/");
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new Kupac
                {
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
                    await _userManager.AddToRoleAsync(user, "Kupac"); 

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
        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return Redirect("/Home/");
            }
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return Redirect("/Home/");
        }

        public async Task<IActionResult> TwoFactorAuth(string statusMessage = "")
        {
            var korisnik = await _userManager.GetUserAsync(User);
            if (korisnik == null)
            {
                return NotFound($"Nema korisnika sa ID-om '{_userManager.GetUserId(User)}'.");
            }
            var model = new TwoFactorAuthVM
            {
                HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(korisnik) != null,
                Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(korisnik),
                IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(korisnik),
                RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(korisnik)
            };
            if (!string.IsNullOrEmpty(statusMessage))
            {
                model.StatusMessage = statusMessage;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ForgetBrowser2FA()
        {
            var korisnik = await _userManager.GetUserAsync(User);
            if (korisnik == null)
            {
                return NotFound($"Nema korisnika sa ID-om '{_userManager.GetUserId(User)}'.");
            }

            await _signInManager.ForgetTwoFactorClientAsync();

            return RedirectToAction(nameof(TwoFactorAuth), new { statusMessage = "Ovaj browser je zaboravljen." });
        }

        public async Task<IActionResult> GenerisiCodove()
        {
            var korisnik = await _userManager.GetUserAsync(User);
            if (korisnik == null)
            {
                return NotFound($"Nema korisnika sa ID-om '{_userManager.GetUserId(User)}'.");
            }

            var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(korisnik);
            if (!isTwoFactorEnabled)
            {
                return RedirectToAction("Index", "Home");
            }

            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(korisnik, 10);
            List<string> codesList = recoveryCodes.ToList();

            return View(codesList);
        }

        public async Task<IActionResult> Disable2FA()
        {
            var korisnik = await _userManager.GetUserAsync(User);
            if (korisnik == null)
            {
                return NotFound($"Nema korisnika sa ID-om '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.SetTwoFactorEnabledAsync(korisnik, false);

            return RedirectToAction(nameof(TwoFactorAuth), new { statusMessage = "2FA je sada ugašena." });
        }
        public async Task<IActionResult> Enable2FA(string statusMessage = "")
        {
            var korisnik = await _userManager.GetUserAsync(User);
            if (korisnik == null)
            {
                return NotFound($"Nema korisnika sa ID-om '{_userManager.GetUserId(User)}'.");
            }

            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(korisnik);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(korisnik);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(korisnik);
            }

            var email = await _userManager.GetEmailAsync(korisnik);

            var model = new Enable2FAVM
            {
                SharedKey = FormatKey(unformattedKey),
                AuthenticatorUri = GenerateQrCodeUri(email, unformattedKey),
                StatusMessage = statusMessage
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Enable2FA(Enable2FAVM model)
        {
            var korisnik = await _userManager.GetUserAsync(User);
            if (korisnik == null)
            {
                return NotFound($"Nema korisnika sa ID-om '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                RedirectToAction(nameof(Enable2FA));
            }

            // Strip spaces and hypens
            var verificationCode = model.VerificationCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                korisnik, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                return RedirectToAction(nameof(Enable2FA), new { statusMessage = "Netačan verifikacijski kod." });
            }

            await _userManager.SetTwoFactorEnabledAsync(korisnik, true);
            var userId = await _userManager.GetUserIdAsync(korisnik);

            if (await _userManager.CountRecoveryCodesAsync(korisnik) == 0)
            {
                return RedirectToAction(nameof(GenerisiCodove));
            }
            else
            {
                return RedirectToAction(nameof(TwoFactorAuth), new { statusMessage = "2FA je podešen." });
            }
        }
        [HttpPost]
        public async Task<IActionResult> LoginRecoveryCode(string RecoveryCode)
        {
            var recoveryCode = RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            if (result.IsLockedOut)
            {
                //Korisnik locked out, nema obavijesti nikakve za ovo za sad
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //Nema obavijesti da je greška napravljena
                return RedirectToAction("Index", "Home");
            }
        }


        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6",
                _urlEncoder.Encode("NamirniceDelivery"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }
    }
}