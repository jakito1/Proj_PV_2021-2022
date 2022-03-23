// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NutriFitWeb.Models;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
***REMOVED***
    /// <summary>
    /// Disable2faModel class, derived from PageModel.
    /// </summary>
    public class Disable2faModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly ILogger<Disable2faModel> _logger;

        /// <summary>
        /// Build the Disable2faModel model to be used when the user wants to disable the two factor authentication in the account profile.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="logger">A generic interface for logging where the category name is derived from this class.</param>
        public Disable2faModel(
            UserManager<UserAccountModel> userManager,
            ILogger<Disable2faModel> logger)
        ***REMOVED***
            _userManager = userManager;
            _logger = logger;
    ***REMOVED***

        /// <summary>
        ///     Gets or sets the temporary string StatusMessage.
        /// </summary>
        [TempData]
        public string StatusMessage ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Handle the Get Request during the Disable2fa process.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Exception thrown when a user tries to disable 2FA on an account with it disabled</exception>
        public async Task<IActionResult> OnGet()
        ***REMOVED***
            UserAccountModel user = await _userManager.GetUserAsync(User);
            if (user is null)
            ***REMOVED***
                return NotFound($"Não foi possível carregar o utilizador com o ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            if (!await _userManager.GetTwoFactorEnabledAsync(user))
            ***REMOVED***
                throw new InvalidOperationException($"Cannot disable 2FA for user as it's not currently enabled.");
        ***REMOVED***

            return Page();
    ***REMOVED***

        /// <summary>
        /// Handle the Post Request during the Disable2fa process.
        /// Tries to disable the two factor authentication.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Exception thrown when an error occurs during the 2FA deactivation</exception>
        public async Task<IActionResult> OnPostAsync()
        ***REMOVED***
            UserAccountModel user = await _userManager.GetUserAsync(User);
            if (user is null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            IdentityResult disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
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
