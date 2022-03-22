// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NutriFitWeb.Models;

namespace NutriFitWeb.Areas.Identity.Pages.Account
***REMOVED***
    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class AccessDeniedModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccountModel> _userManager;

        public AccessDeniedModel(UserManager<UserAccountModel> userManager)
        ***REMOVED***
            _userManager = userManager;
    ***REMOVED***

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IActionResult OnGet()
        ***REMOVED***
            if (User.Identity.IsAuthenticated)
            ***REMOVED***
                return Page();
        ***REMOVED***
            return LocalRedirect("~/Identity/Account/Login");
    ***REMOVED***
***REMOVED***
***REMOVED***
