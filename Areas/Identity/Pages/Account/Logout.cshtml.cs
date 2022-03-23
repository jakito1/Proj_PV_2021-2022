// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NutriFitWeb.Models;

namespace NutriFitWeb.Areas.Identity.Pages.Account
***REMOVED***
    /// <summary>
    /// LogoutModel class, derived from PageModel.
    /// </summary>
    public class LogoutModel : PageModel
    ***REMOVED***
        private readonly SignInManager<UserAccountModel> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        /// <summary>
        /// Build the LogoutModel model to be used when the user decides to logout.
        /// </summary>
        /// <param name="signInManager">Provides the APIs for user sign in using the UserAccountModel.</param>
        /// <param name="logger">A generic interface for logging where the category name is derived from this class.</param>
        public LogoutModel(SignInManager<UserAccountModel> signInManager, ILogger<LogoutModel> logger)
        ***REMOVED***
            _signInManager = signInManager;
            _logger = logger;
    ***REMOVED***

        /// <summary>
        /// Handles the Post Request during the logout process.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPost(string returnUrl = null)
        ***REMOVED***
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl is not null)
            ***REMOVED***
                return LocalRedirect(returnUrl);
        ***REMOVED***
            else
            ***REMOVED***
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
        ***REMOVED***
    ***REMOVED***
***REMOVED***
***REMOVED***
