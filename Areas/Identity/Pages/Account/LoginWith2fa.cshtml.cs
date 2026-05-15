// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SmartPark.Models;

namespace SmartPark.Areas.Identity.Pages.Account
{
    public class LoginWith2faModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginWith2faModel> _logger;

        public LoginWith2faModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<LoginWith2faModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required(ErrorMessage = "Vnos kode za preverjanje pristnosti je obvezen.")]
            [StringLength(
                7,
                ErrorMessage = "Polje {0} mora vsebovati najmanj {2} in največ {1} znakov.",
                MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Koda iz avtentikatorja")]
            public string TwoFactorCode { get; set; }

            [Display(Name = "Zapomni si to napravo")]
            public bool RememberMachine { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
        {
            // Preverimo, da je uporabnik najprej uspešno vnesel e-poštni naslov in geslo.
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException(
                    "Ni mogoče naložiti uporabnika za dvostopenjsko preverjanje pristnosti.");
            }

            ReturnUrl = returnUrl;
            RememberMe = rememberMe;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException(
                    "Ni mogoče naložiti uporabnika za dvostopenjsko preverjanje pristnosti.");
            }

            var authenticatorCode = Input.TwoFactorCode
                .Replace(" ", string.Empty)
                .Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(
                authenticatorCode,
                rememberMe,
                Input.RememberMachine);

            var userId = await _userManager.GetUserIdAsync(user);

            if (result.Succeeded)
            {
                _logger.LogInformation(
                    "Uporabnik z ID-jem '{UserId}' se je uspešno prijavil z dvostopenjskim preverjanjem pristnosti.",
                    user.Id);

                return LocalRedirect(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning(
                    "Račun uporabnika z ID-jem '{UserId}' je zaklenjen.",
                    user.Id);

                return RedirectToPage("./Lockout");
            }
            else
            {
                _logger.LogWarning(
                    "Uporabnik z ID-jem '{UserId}' je vnesel neveljavno kodo za preverjanje pristnosti.",
                    user.Id);

                ModelState.AddModelError(
                    string.Empty,
                    "Vnesena koda za preverjanje pristnosti ni veljavna.");

                return Page();
            }
        }
    }
}
