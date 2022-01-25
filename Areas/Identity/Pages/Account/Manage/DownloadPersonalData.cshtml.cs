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
using NutriFitWeb.Models;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
***REMOVED***
    /// <summary>
    /// DownloadPersonalDataModel class, derived from PageModel.
    /// </summary>
    public class DownloadPersonalDataModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly ILogger<DownloadPersonalDataModel> _logger;

        /// <summary>
        /// Build the DownloadPersonalDataModel model to be used when the user wants to view the page where it's possible to download the personal data in the account profile.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="logger">A generic interface for logging where the category name is derived from this class.</param>
        public DownloadPersonalDataModel(
            UserManager<UserAccountModel> userManager,
            ILogger<DownloadPersonalDataModel> logger)
        ***REMOVED***
            _userManager = userManager;
            _logger = logger;
    ***REMOVED***

        /// <summary>
        /// Handle the Get Request during the DownloadPersonalData process.
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        ***REMOVED***
            return NotFound();
    ***REMOVED***

        /// <summary>
        /// Handle the Post Request during the DownloadPersonalData process.
        /// Tries to download the personal user data.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            _logger.LogInformation("User with ID '***REMOVED***UserId***REMOVED***' asked for their personal data.", _userManager.GetUserId(User));

            // Only include personal data for download
            var personalData = new Dictionary<string, string>();
            var personalDataProps = typeof(UserAccountModel).GetProperties().Where(
                            prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
            foreach (var p in personalDataProps)
            ***REMOVED***
                personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
        ***REMOVED***

            var logins = await _userManager.GetLoginsAsync(user);
            foreach (var l in logins)
            ***REMOVED***
                personalData.Add($"***REMOVED***l.LoginProvider***REMOVED*** external login provider key", l.ProviderKey);
        ***REMOVED***

            personalData.Add($"Authenticator Key", await _userManager.GetAuthenticatorKeyAsync(user));

            Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
            return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");
    ***REMOVED***
***REMOVED***
***REMOVED***
