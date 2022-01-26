// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NutriFitWeb.Models;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
***REMOVED***
    /// <summary>
    /// ShowRecoveryCodesModel class, derived from PageModel.
    /// </summary>
    public class ShowRecoveryCodesModel : PageModel
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
        /// Handles the Get Request during the code show up process.
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        ***REMOVED***
            if (RecoveryCodes == null || RecoveryCodes.Length == 0)
            ***REMOVED***
                return RedirectToPage("./TwoFactorAuthentication");
        ***REMOVED***

            return Page();
    ***REMOVED***
***REMOVED***
***REMOVED***
