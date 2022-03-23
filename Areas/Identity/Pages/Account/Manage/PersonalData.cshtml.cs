// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NutriFitWeb.Models;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
***REMOVED***
    //[Authorize(Roles = "client")]
    /// <summary>
    /// PersonalDataModel class, derived from PageModel.
    /// </summary>
    public class PersonalDataModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;

        /// <summary>
        /// Build the PersonalDataModel model to be used when the user wants to view the page where it's possible to download the personal data from the account profile.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="logger">A generic interface for logging where the category name is derived from this class.</param>
        public PersonalDataModel(
            UserManager<UserAccountModel> userManager,
            ILogger<PersonalDataModel> logger)
        ***REMOVED***
            _userManager = userManager;
            _logger = logger;
    ***REMOVED***

        /// <summary>
        /// Handle the Get Request during the page creation process.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGet()
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            return Page();
    ***REMOVED***
***REMOVED***
***REMOVED***
