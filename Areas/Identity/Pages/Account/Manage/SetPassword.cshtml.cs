// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NutriFitWeb.Areas.Identity.Data;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
***REMOVED***
    public class SetPasswordModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;

        public SetPasswordModel(
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
        [BindProperty]
        public InputModel Input ***REMOVED*** get; set; ***REMOVED***

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
        public class InputModel
        ***REMOVED***
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The ***REMOVED***0***REMOVED*** must be at least ***REMOVED***2***REMOVED*** and at max ***REMOVED***1***REMOVED*** characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword ***REMOVED*** get; set; ***REMOVED***

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword ***REMOVED*** get; set; ***REMOVED***
    ***REMOVED***

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
