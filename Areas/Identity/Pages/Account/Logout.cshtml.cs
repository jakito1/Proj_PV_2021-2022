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
    public class LogoutModel : PageModel
    ***REMOVED***
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<UserAccount> signInManager, ILogger<LogoutModel> logger)
        ***REMOVED***
            _signInManager = signInManager;
            _logger = logger;
    ***REMOVED***

        public async Task<IActionResult> OnPost(string returnUrl = null)
        ***REMOVED***
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
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
