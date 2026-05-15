// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SmartPark.Models;

namespace SmartPark.Areas.Identity.Pages.Account.Manage
{
    public class DownloadPersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DownloadPersonalDataModel> _logger;

        public DownloadPersonalDataModel(
            UserManager<ApplicationUser> userManager,
            ILogger<DownloadPersonalDataModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Ni mogoče naložiti uporabnika z ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation(
                "Uporabnik z ID '{UserId}' je zahteval svoje osebne podatke.",
                _userManager.GetUserId(User));

            // Vključimo samo osebne podatke za prenos
            var personalData = new Dictionary<string, string>();

            var personalDataProps = typeof(ApplicationUser)
                .GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));

            foreach (var p in personalDataProps)
            {
                personalData.Add(
                    p.Name,
                    p.GetValue(user)?.ToString() ?? "null");
            }

            var logins = await _userManager.GetLoginsAsync(user);

            foreach (var l in logins)
            {
                personalData.Add(
                    $"{l.LoginProvider} ključ zunanje prijave",
                    l.ProviderKey);
            }

            personalData.Add(
                "Avtentikacijski ključ",
                await _userManager.GetAuthenticatorKeyAsync(user));

            Response.Headers.TryAdd(
                "Content-Disposition",
                "attachment; filename=OsebniPodatki.json");

            return new FileContentResult(
                JsonSerializer.SerializeToUtf8Bytes(personalData),
                "application/json");
        }
    }
}