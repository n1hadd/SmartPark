// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SmartPark.Models;

namespace SmartPark.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Vnos trenutnega gesla je obvezen.")]
            [DataType(DataType.Password)]
            [Display(Name = "Trenutno geslo")]
            public string OldPassword { get; set; }

            [Required(ErrorMessage = "Vnos novega gesla je obvezen.")]
            [StringLength(
                100,
                ErrorMessage = "Novo geslo mora biti dolgo vsaj {2} in največ {1} znakov.",
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Novo geslo")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potrditev novega gesla")]
            [Compare("NewPassword", ErrorMessage = "Novo geslo in potrditev se ne ujemata.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Uporabnika z ID '{_userManager.GetUserId(User)}' ni mogoče najti.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);

            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Uporabnika z ID '{_userManager.GetUserId(User)}' ni mogoče najti.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(
                user,
                Input.OldPassword,
                Input.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);

            _logger.LogInformation("Uporabnik je uspešno spremenil geslo.");

            StatusMessage = "Vaše geslo je bilo uspešno spremenjeno.";

            return RedirectToPage();
        }
    }
}