// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NutriFitWeb.Models;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
***REMOVED***
    /// <summary>
    /// SetPasswordModel class, derived from PageModel.
    /// </summary>
    public class SetPasswordModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly SignInManager<UserAccountModel> _signInManager;

        /// <summary>
        /// Build the SetPasswordModel model to be used when the user wants to set a password from the account profile.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="signInManager">Provides the APIs for user sign in using the UserAccountModel.</param>
        public SetPasswordModel(
            UserManager<UserAccountModel> userManager,
            SignInManager<UserAccountModel> signInManager)
        ***REMOVED***
            _userManager = userManager;
            _signInManager = signInManager;
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
            ///     Gets or sets the new password inputed by the user.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The ***REMOVED***0***REMOVED*** must be at least ***REMOVED***2***REMOVED*** and at max ***REMOVED***1***REMOVED*** characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword ***REMOVED*** get; set; ***REMOVED***

            /// <summary>
            ///     Gets or sets the repeated new password inputed by the user.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword ***REMOVED*** get; set; ***REMOVED***
    ***REMOVED***

        /// <summary>
        /// Handle the Get Request during the password setup process.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync()
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            var hasPassword = await _userManager.HasPasswordAsync(user);

            if (hasPassword)
            ***REMOVED***
                return RedirectToPage("./ChangePassword");
        ***REMOVED***

            return Page();
    ***REMOVED***

        /// <summary>
        /// Handle the Post Request during the 2FA reset process.
        /// Tries to setup the inputed password as a new password.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        ***REMOVED***
            if (!ModelState.IsValid)
            ***REMOVED***
                return Page();
        ***REMOVED***

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
            if (!addPasswordResult.Succeeded)
            ***REMOVED***
                foreach (var error in addPasswordResult.Errors)
                ***REMOVED***
                    ModelState.AddModelError(string.Empty, error.Description);
            ***REMOVED***
                return Page();
        ***REMOVED***

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your password has been set.";

            return RedirectToPage();
    ***REMOVED***
***REMOVED***
***REMOVED***
