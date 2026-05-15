// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SmartPark.Models;

namespace SmartPark.Areas.Identity.Pages.Account.Manage
{
    public class GenerateRecoveryCodesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<GenerateRecoveryCodesModel> _logger;

        public GenerateRecoveryCodesModel(
            UserManager<ApplicationUser> userManager,
            ILogger<GenerateRecoveryCodesModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [TempData]
        public string[] RecoveryCodes { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Ni mogoče naložiti uporabnika z ID '{_userManager.GetUserId(User)}'.");
            }

            var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);

            if (!isTwoFactorEnabled)
            {
                throw new InvalidOperationException(
                    "Ni mogoče ustvariti obnovitvenih kod, ker 2FA ni omogočen.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Ni mogoče naložiti uporabnika z ID '{_userManager.GetUserId(User)}'.");
            }

            var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);

            if (!isTwoFactorEnabled)
            {
                throw new InvalidOperationException(
                    "Ni mogoče ustvariti obnovitvenih kod, ker 2FA ni omogočen.");
            }

            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

            RecoveryCodes = recoveryCodes.ToArray();

            _logger.LogInformation(
                "Uporabnik z ID '{UserId}' je ustvaril nove 2FA obnovitvene kode.",
                userId);

            StatusMessage = "Ustvarili ste nove obnovitvene kode.";

            return RedirectToPage("./ShowRecoveryCodes");
        }
    }
}