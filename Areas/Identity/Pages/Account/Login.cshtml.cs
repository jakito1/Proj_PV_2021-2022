// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NutriFitWeb.Areas.Identity.Data;

namespace NutriFitWeb.Areas.Identity.Pages.Account
***REMOVED***
    public class LoginModel : PageModel
    ***REMOVED***
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<UserAccount> signInManager, ILogger<LoginModel> logger)
        ***REMOVED***
            _signInManager = signInManager;
            _logger = logger;
    ***REMOVED***

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        ***REMOVED***
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email ***REMOVED*** get; set; ***REMOVED***

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password ***REMOVED*** get; set; ***REMOVED***

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe ***REMOVED*** get; set; ***REMOVED***
    ***REMOVED***

        public async Task OnGetAsync(string returnUrl = null)
        ***REMOVED***
            if (!string.IsNullOrEmpty(ErrorMessage))
            ***REMOVED***
                ModelState.AddModelError(string.Empty, ErrorMessage);
        ***REMOVED***

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
    ***REMOVED***

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        ***REMOVED***
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            ***REMOVED***
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                ***REMOVED***
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
            ***REMOVED***
                if (result.RequiresTwoFactor)
                ***REMOVED***
                    return RedirectToPage("./LoginWith2fa", new ***REMOVED*** ReturnUrl = returnUrl, RememberMe = Input.RememberMe ***REMOVED***);
            ***REMOVED***
                if (result.IsLockedOut)
                ***REMOVED***
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
            ***REMOVED***
                else
                ***REMOVED***
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
            ***REMOVED***
        ***REMOVED***

            // If we got this far, something failed, redisplay form
            return Page();
    ***REMOVED***
***REMOVED***
***REMOVED***
