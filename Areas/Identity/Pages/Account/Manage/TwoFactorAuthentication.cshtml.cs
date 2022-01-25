// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NutriFitWeb.Models;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
***REMOVED***
    /// <summary>
    /// TwoFactorAuthenticationModel class, derived from PageModel.
    /// </summary>
    public class TwoFactorAuthenticationModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly SignInManager<UserAccountModel> _signInManager;
        private readonly ILogger<TwoFactorAuthenticationModel> _logger;

        /// <summary>
        /// Build the TwoFactorAuthenticationModel model to be used when the user wants to view a page where it's requested a 2FA code.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="signInManager">Provides the APIs for user sign in using the UserAccountModel.</param>
        /// <param name="logger">A generic interface for logging where the category name is derived from this class.</param>
        public TwoFactorAuthenticationModel(
            UserManager<UserAccountModel> userManager, SignInManager<UserAccountModel> signInManager, ILogger<TwoFactorAuthenticationModel> logger)
        ***REMOVED***
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
    ***REMOVED***

        /// <summary>
        ///     Gets or sets the flag containing whether the user has 2FA setup.
        /// </summary>
        public bool HasAuthenticator ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets the amount of recovery codes left.
        /// </summary>
        public int RecoveryCodesLeft ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///      Gets or sets the flag containing whether the user has 2FA enabled.
        /// </summary>
        [BindProperty]
        public bool Is2faEnabled ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets the flag containing whether the user has the current machine remembered.
        /// </summary>
        public bool IsMachineRemembered ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets the temporary string StatusMessage.
        /// </summary>
        [TempData]
        public string StatusMessage ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Handles the Get Request during the 2FA code request process.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync()
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null;
            Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user);
            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user);

            return Page();
    ***REMOVED***

        /// <summary>
        /// Handles the Post Request during the 2FA code request process.
        /// Tries to forget a machine, requiring a 2FA code on the next login.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            await _signInManager.ForgetTwoFactorClientAsync();
            StatusMessage = "The current browser has been forgotten. When you login again from this browser you will be prompted for your 2fa code.";
            return RedirectToPage();
    ***REMOVED***
***REMOVED***
***REMOVED***
