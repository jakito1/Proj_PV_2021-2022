// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NutriFitWeb.Areas.Identity.Data;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
***REMOVED***
    public class Disable2faModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccount> _userManager;
        private readonly ILogger<Disable2faModel> _logger;

        public Disable2faModel(
            UserManager<UserAccount> userManager,
            ILogger<Disable2faModel> logger)
        ***REMOVED***
            _userManager = userManager;
            _logger = logger;
    ***REMOVED***

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage ***REMOVED*** get; set; ***REMOVED***

        public async Task<IActionResult> OnGet()
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            if (!await _userManager.GetTwoFactorEnabledAsync(user))
            ***REMOVED***
                throw new InvalidOperationException($"Cannot disable 2FA for user as it's not currently enabled.");
        ***REMOVED***

            return Page();
    ***REMOVED***

        public async Task<IActionResult> OnPostAsync()
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            ***REMOVED***
                throw new InvalidOperationException($"Unexpected error occurred disabling 2FA.");
        ***REMOVED***

            _logger.LogInformation("User with ID '***REMOVED***UserId***REMOVED***' has disabled 2fa.", _userManager.GetUserId(User));
            StatusMessage = "2fa has been disabled. You can reenable 2fa when you setup an authenticator app";
            return RedirectToPage("./TwoFactorAuthentication");
    ***REMOVED***
***REMOVED***
***REMOVED***
