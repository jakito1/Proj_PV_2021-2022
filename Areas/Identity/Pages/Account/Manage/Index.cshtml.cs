// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NutriFitWeb.Areas.Identity.Data;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
***REMOVED***
    public class IndexModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;

        public IndexModel(
            UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager)
        ***REMOVED***
            _userManager = userManager;
            _signInManager = signInManager;
    ***REMOVED***

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage ***REMOVED*** get; set; ***REMOVED***

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
        public class InputModel
        ***REMOVED***
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber ***REMOVED*** get; set; ***REMOVED***
    ***REMOVED***

        private async Task LoadAsync(UserAccount user)
        ***REMOVED***
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            ***REMOVED***
                PhoneNumber = phoneNumber
        ***REMOVED***;
    ***REMOVED***

        public async Task<IActionResult> OnGetAsync()
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            await LoadAsync(user);
            return Page();
    ***REMOVED***

        public async Task<IActionResult> OnPostAsync()
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            if (!ModelState.IsValid)
            ***REMOVED***
                await LoadAsync(user);
                return Page();
        ***REMOVED***

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            ***REMOVED***
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
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
