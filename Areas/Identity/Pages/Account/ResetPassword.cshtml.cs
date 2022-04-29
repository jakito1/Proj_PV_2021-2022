// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using NutriFitWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NutriFitWeb.Areas.Identity.Pages.Account
***REMOVED***
    public class ResetPasswordModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccountModel> _userManager;

        public ResetPasswordModel(UserManager<UserAccountModel> userManager)
        ***REMOVED***
            _userManager = userManager;
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
            [StringLength(100, ErrorMessage = "The ***REMOVED***0***REMOVED*** must be at least ***REMOVED***2***REMOVED*** and at max ***REMOVED***1***REMOVED*** characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password ***REMOVED*** get; set; ***REMOVED***

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword ***REMOVED*** get; set; ***REMOVED***

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            public string Code ***REMOVED*** get; set; ***REMOVED***

    ***REMOVED***

        public IActionResult OnGet(string code = null)
        ***REMOVED***
            if (code == null)
            ***REMOVED***
                return BadRequest("A code must be supplied for password reset.");
        ***REMOVED***
            else
            ***REMOVED***
                Input = new InputModel
                ***REMOVED***
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
            ***REMOVED***;
                return Page();
        ***REMOVED***
    ***REMOVED***

        public async Task<IActionResult> OnPostAsync()
        ***REMOVED***
            if (!ModelState.IsValid)
            ***REMOVED***
                return Page();
        ***REMOVED***

            UserAccountModel user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            ***REMOVED***
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
        ***REMOVED***

            IdentityResult result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            ***REMOVED***
                return RedirectToPage("./ResetPasswordConfirmation");
        ***REMOVED***

            foreach (IdentityError error in result.Errors)
            ***REMOVED***
                ModelState.AddModelError(string.Empty, error.Description);
        ***REMOVED***
            return Page();
    ***REMOVED***
***REMOVED***
***REMOVED***
