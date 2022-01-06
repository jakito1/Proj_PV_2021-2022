// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using NutriFitWeb.Areas.Identity.Data;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
***REMOVED***
    public class EmailModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly IEmailSender _emailSender;

        public EmailModel(
            UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager,
            IEmailSender emailSender)
        ***REMOVED***
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
    ***REMOVED***

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Email ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool IsEmailConfirmed ***REMOVED*** get; set; ***REMOVED***

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
            [Required]
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail ***REMOVED*** get; set; ***REMOVED***
    ***REMOVED***

        private async Task LoadAsync(UserAccount user)
        ***REMOVED***
            var email = await _userManager.GetEmailAsync(user);
            Email = email;

            Input = new InputModel
            ***REMOVED***
                NewEmail = email,
        ***REMOVED***;

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
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

        public async Task<IActionResult> OnPostChangeEmailAsync()
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

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            ***REMOVED***
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new ***REMOVED*** area = "Identity", userId = userId, email = Input.NewEmail, code = code ***REMOVED***,
                    protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    Input.NewEmail,
                    "Confirm your email",
                    $"Please confirm your account by <a href='***REMOVED***HtmlEncoder.Default.Encode(callbackUrl)***REMOVED***'>clicking here</a>.");

                StatusMessage = "Confirmation link to change email sent. Please check your email.";
                return RedirectToPage();
        ***REMOVED***

            StatusMessage = "Your email is unchanged.";
            return RedirectToPage();
    ***REMOVED***

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
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

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new ***REMOVED*** area = "Identity", userId = userId, code = code ***REMOVED***,
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='***REMOVED***HtmlEncoder.Default.Encode(callbackUrl)***REMOVED***'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
    ***REMOVED***
***REMOVED***
***REMOVED***
