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
    /// IndexModel class, derived from PageModel.
    /// </summary>
    public class IndexModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly SignInManager<UserAccountModel> _signInManager;

        /// <summary>
        /// Build the IndexModel model to be used when the user wants to view the page where it's possible to add the phone number in the account profile.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="signInManager">Provides the APIs for user sign in using the UserAccountModel.</param>
        public IndexModel(
            UserManager<UserAccountModel> userManager,
            SignInManager<UserAccountModel> signInManager)
        ***REMOVED***
            _userManager = userManager;
            _signInManager = signInManager;
    ***REMOVED***

        /// <summary>
        ///     Gets or sets the Username
        /// </summary>
        public string Username ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets the temporary string StatusMessage.
        /// </summary>
        [TempData]
        public string StatusMessage ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets or sets the data containing the user input.
        /// </summary>
        [BindProperty]
        public InputModel Input ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Inner class specifying what data the user can input.
        /// </summary>
        public class InputModel
        ***REMOVED***
            /// <summary>
            ///     Gets or sets the phone number inputed by the user.
            /// </summary>
            [Phone]
            [Display(Name = "Número de Telemóvel")]
            public string PhoneNumber ***REMOVED*** get; set; ***REMOVED***
    ***REMOVED***

        private async Task LoadAsync(UserAccountModel user)
        ***REMOVED***
            string userName = await _userManager.GetUserNameAsync(user);
            string phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            ***REMOVED***
                PhoneNumber = phoneNumber
        ***REMOVED***;
    ***REMOVED***

        /// <summary>
        /// Handle the Get Request during the phone number addition process.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync()
        ***REMOVED***
            UserAccountModel user = await _userManager.GetUserAsync(User);
            if (user is null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            await LoadAsync(user);
            return Page();
    ***REMOVED***

        /// <summary>
        /// Handle the Post Request during the phone number addition process.
        /// Tries to add the inputed phone number to the user profile.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        ***REMOVED***
            UserAccountModel user = await _userManager.GetUserAsync(User);
            if (user is null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            if (!ModelState.IsValid)
            ***REMOVED***
                await LoadAsync(user);
                return Page();
        ***REMOVED***

            string phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            ***REMOVED***
                IdentityResult setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                ***REMOVED***
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
            ***REMOVED***
        ***REMOVED***

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
    ***REMOVED***
***REMOVED***
***REMOVED***
