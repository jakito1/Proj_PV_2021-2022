// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NutriFitWeb.Models;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
***REMOVED***
    /// <summary>
    /// ExternalLoginsModel class, derived from PageModel.
    /// </summary>
    public class ExternalLoginsModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly SignInManager<UserAccountModel> _signInManager;
        private readonly IUserStore<UserAccountModel> _userStore;

        /// <summary>
        /// Build the ExternalLoginsModel model to be used when the user wants to view the page where it's possible to use an external login.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="signInManager">Provides the APIs for user sign in using the UserAccountModel.</param>
        /// <param name="userStore">Provides an abstraction for a store which manages the UserAccountModel.</param>
        public ExternalLoginsModel(
            UserManager<UserAccountModel> userManager,
            SignInManager<UserAccountModel> signInManager,
            IUserStore<UserAccountModel> userStore)
        ***REMOVED***
            _userManager = userManager;
            _signInManager = signInManager;
            _userStore = userStore;
    ***REMOVED***

        /// <summary>
        ///     Gets or sets a list with the current application supported external logins
        /// </summary>
        public IList<UserLoginInfo> CurrentLogins ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets a list with all the external logins theoretically supported.
        /// </summary>
        public IList<AuthenticationScheme> OtherLogins ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets the flag whether to show the ShowRemoveButton.
        /// </summary>
        public bool ShowRemoveButton ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets the temporary string StatusMessage.
        /// </summary>
        [TempData]
        public string StatusMessage ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Tries to show the possible external login options.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync()
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            CurrentLogins = await _userManager.GetLoginsAsync(user);
            OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();

            string passwordHash = null;
            if (_userStore is IUserPasswordStore<UserAccountModel> userPasswordStore)
            ***REMOVED***
                passwordHash = await userPasswordStore.GetPasswordHashAsync(user, HttpContext.RequestAborted);
        ***REMOVED***

            ShowRemoveButton = passwordHash != null || CurrentLogins.Count > 1;
            return Page();
    ***REMOVED***

        /// <summary>
        /// Tries to remove an external login information from the user account.
        /// </summary>
        /// <param name="loginProvider">string with the provider name</param>
        /// <param name="providerKey">string with the provider key</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostRemoveLoginAsync(string loginProvider, string providerKey)
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
            if (!result.Succeeded)
            ***REMOVED***
                StatusMessage = "The external login was not removed.";
                return RedirectToPage();
        ***REMOVED***

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "The external login was removed.";
            return RedirectToPage();
    ***REMOVED***

        /// <summary>
        /// Tries to redirect an user to the external login link.
        /// </summary>
        /// <param name="provider">string with the provider name</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
        ***REMOVED***
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Page("./ExternalLogins", pageHandler: "LinkLoginCallback");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return new ChallengeResult(provider, properties);
    ***REMOVED***

        /// <summary>
        /// Tries to add an external login to a user.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs while obtaining the external logins informations</exception>
        public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            var userId = await _userManager.GetUserIdAsync(user);
            var info = await _signInManager.GetExternalLoginInfoAsync(userId);
            if (info == null)
            ***REMOVED***
                throw new InvalidOperationException($"Unexpected error occurred loading external login info.");
        ***REMOVED***

            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            ***REMOVED***
                StatusMessage = "The external login was not added. External logins can only be associated with one account.";
                return RedirectToPage();
        ***REMOVED***

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            StatusMessage = "The external login was added.";
            return RedirectToPage();
    ***REMOVED***
***REMOVED***
***REMOVED***
