// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NutriFitWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
***REMOVED***
    /// <summary>
    /// ChangePasswordModel class, derived from PageModel.
    /// </summary>
    public class ChangePasswordModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly SignInManager<UserAccountModel> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        /// <summary>
        /// Build the ChangePasswordModel model to be used when the user wants to view the page where it's possible to change the password in the account profile.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="signInManager">Provides the APIs for user sign in using the UserAccountModel.</param>
        /// <param name="logger">A generic interface for logging where the category name is derived from this class.</param>
        public ChangePasswordModel(
            UserManager<UserAccountModel> userManager,
            SignInManager<UserAccountModel> signInManager,
            ILogger<ChangePasswordModel> logger)
        ***REMOVED***
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
    ***REMOVED***

        /// <summary>
        /// Gets or sets the data containing the user input.
        /// </summary>
        [BindProperty]
        public InputModel Input ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets the temporary string StatusMessage.
        /// </summary>
        [TempData]
        public string StatusMessage ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Inner class specifying what data the user can input.
        /// </summary>
        public class InputModel
        ***REMOVED***
            /// <summary>
            ///     Gets or sets the old user password (the one that's going to be changed)
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Palavra-passe Atual")]
            public string OldPassword ***REMOVED*** get; set; ***REMOVED***

            /// <summary>
            ///     Gets or sets the new user password.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "A ***REMOVED***0***REMOVED*** deve conter pelo menos ***REMOVED***2***REMOVED*** e no máximo ***REMOVED***1***REMOVED*** caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Nova palavra-passe")]
            public string NewPassword ***REMOVED*** get; set; ***REMOVED***

            /// <summary>
            ///     Gets or sets the repeated new user password.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirme a nova palavra-passe")]
            [Compare("NewPassword", ErrorMessage = "A nova palavra-passe e a confirmação da nova palavra-passe não correspondem.")]
            public string ConfirmPassword ***REMOVED*** get; set; ***REMOVED***
    ***REMOVED***

        /// <summary>
        /// Handles the Get Request during the ChangePassword process.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync()
        ***REMOVED***
            UserAccountModel user = await _userManager.GetUserAsync(User);
            if (user is null)
            ***REMOVED***
                return NotFound($"Não foi possível carregar o utilizador com o ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            bool hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            ***REMOVED***
                return RedirectToPage("./SetPassword");
        ***REMOVED***

            return Page();
    ***REMOVED***

        /// <summary>
        /// Handles the Post Request during the ChangePassword process.
        /// Will try to change the old password to a new one, only if the new one respects some restraints and only if the NewPassword and ConfirmPassword are the same.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        ***REMOVED***
            if (!ModelState.IsValid)
            ***REMOVED***
                return Page();
        ***REMOVED***

            UserAccountModel user = await _userManager.GetUserAsync(User);
            if (user is null)
            ***REMOVED***
                return NotFound($"Não foi possível carregar o utilizador com o ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            IdentityResult changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            ***REMOVED***
                foreach (IdentityError error in changePasswordResult.Errors)
                ***REMOVED***
                    ModelState.AddModelError(string.Empty, error.Description);
            ***REMOVED***
                return Page();
        ***REMOVED***

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("Palavra-passe atualizada com sucesso");
            StatusMessage = "A sua palavra-passe foi atualizada.";

            return RedirectToPage();
    ***REMOVED***
***REMOVED***
***REMOVED***
