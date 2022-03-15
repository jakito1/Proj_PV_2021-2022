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
using NutriFitWeb.Models;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
***REMOVED***
    /// <summary>
    /// GenerateRecoveryCodesModel class, derived from PageModel.
    /// </summary>
    public class GenerateRecoveryCodesModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly ILogger<GenerateRecoveryCodesModel> _logger;

        /// <summary>
        /// Build the GenerateRecoveryCodesModel model to be used when the user generates the recovery codes in the account profile.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="logger">A generic interface for logging where the category name is derived from this class.</param>
        public GenerateRecoveryCodesModel(
            UserManager<UserAccountModel> userManager,
            ILogger<GenerateRecoveryCodesModel> logger)
        ***REMOVED***
            _userManager = userManager;
            _logger = logger;
    ***REMOVED***

        /// <summary>
        ///     Gets or sets a temporary string array with the RecoveryCodes
        /// </summary>
        [TempData]
        public string[] RecoveryCodes ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets the temporary string StatusMessage.
        /// </summary>
        [TempData]
        public string StatusMessage ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Handle the Get Request during the recovery codes creation process.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Exception thrown when the user doesn't have 2FA enabled.</exception>
        public async Task<IActionResult> OnGetAsync()
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            if (!isTwoFactorEnabled)
            ***REMOVED***
                throw new InvalidOperationException($"Cannot generate recovery codes for user because they do not have 2FA enabled.");
        ***REMOVED***

            return Page();
    ***REMOVED***

        /// <summary>
        /// Handle the Post Request during the recovery codes creation process.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Exception thrown when the user doesn't have 2FA enabled.</exception>
        public async Task<IActionResult> OnPostAsync()
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!isTwoFactorEnabled)
            ***REMOVED***
                throw new InvalidOperationException($"Cannot generate recovery codes for user as they do not have 2FA enabled.");
        ***REMOVED***

            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            RecoveryCodes = recoveryCodes.ToArray();

            _logger.LogInformation("User with ID '***REMOVED***UserId***REMOVED***' has generated new 2FA recovery codes.", userId);
            StatusMessage = "You have generated new recovery codes.";
            return RedirectToPage("./ShowRecoveryCodes");
    ***REMOVED***
***REMOVED***
***REMOVED***
