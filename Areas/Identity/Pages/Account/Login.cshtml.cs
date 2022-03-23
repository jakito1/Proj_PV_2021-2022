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
using NutriFitWeb.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace NutriFitWeb.Areas.Identity.Pages.Account
***REMOVED***
    /// <summary>
    /// LoginModel class, derived from PageModel.
    /// </summary>
    public class LoginModel : PageModel
    ***REMOVED***
        private readonly SignInManager<UserAccountModel> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        /// <summary>
        /// Build the LoginModel model to be used when the user tries to login.
        /// </summary>
        /// <param name="signInManager">Provides the APIs for user sign in using the UserAccountModel.</param>
        /// <param name="logger">A generic interface for logging where the category name is derived from this class.</param>
        public LoginModel(SignInManager<UserAccountModel> signInManager, ILogger<LoginModel> logger)
        ***REMOVED***
            _signInManager = signInManager;
            _logger = logger;
    ***REMOVED***

        /// <summary>
        /// Gets or sets the data containing the user input.
        /// </summary>
        [BindProperty]
        public InputModel Input ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets or sets a list with the ExternalLogins.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets the ReturnUrl.
        /// </summary>
        public string ReturnUrl ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets the temporary string containing the ErrorMessage.
        /// </summary>
        [TempData]
        public string ErrorMessage ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Inner class specifying what data the user can input.
        /// </summary>
        public class InputModel
        ***REMOVED***
            /// <summary>
            /// Gets or sets the Email inputed by the user.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email ***REMOVED*** get; set; ***REMOVED***

            /// <summary>
            ///     Gets or sets the Password inputed by the user.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password ***REMOVED*** get; set; ***REMOVED***

            /// <summary>
            ///     Gets or sets if the user selected the "Remember me?" option.
            /// </summary>
            [Display(Name = "Lembrar conta?")]
            public bool RememberMe ***REMOVED*** get; set; ***REMOVED***
    ***REMOVED***

        /// <summary>
        /// Handle the Get Request during the Login process.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Handles the Post Request during the Login process.
        /// Tries to login the user.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        ***REMOVED***
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            ***REMOVED***
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var user = _signInManager.UserManager.Users.Where(u => u.Email == Input.Email).FirstOrDefault();

                var result = SignInResult.Failed;
                if (user is not null)
                ***REMOVED***
                    result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: false);
            ***REMOVED***;
                
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
                    ModelState.AddModelError(string.Empty, "Tentativa de Login inválida.");
                    return Page();
            ***REMOVED***
        ***REMOVED***

            // If we got this far, something failed, redisplay form
            return Page();
    ***REMOVED***
***REMOVED***
***REMOVED***
