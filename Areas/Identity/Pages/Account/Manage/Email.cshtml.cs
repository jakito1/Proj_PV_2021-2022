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
using NutriFitWeb.Models;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
***REMOVED***
    /// <summary>
    /// EmailModel class, derived from PageModel.
    /// </summary>
    public class EmailModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly SignInManager<UserAccountModel> _signInManager;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Build the EmailModel model to be used when the user wants to view the page where it's possible to change the email in the account profile.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="signInManager">Provides the APIs for user sign in using the UserAccountModel.</param>
        /// <param name="emailSender">Microsoft EmailSender interface.</param>
        public EmailModel(
            UserManager<UserAccountModel> userManager,
            SignInManager<UserAccountModel> signInManager,
            IEmailSender emailSender)
        ***REMOVED***
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
    ***REMOVED***

        /// <summary>
        /// Gets or sets the user's Email.
        /// </summary>
        public string Email ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets the flag whether the email is confirmed.
        /// </summary>
        public bool IsEmailConfirmed ***REMOVED*** get; set; ***REMOVED***

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
            ///     Gets or sets the new email inputed by the user.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Novo Email")]
            public string NewEmail ***REMOVED*** get; set; ***REMOVED***
    ***REMOVED***

        private async Task LoadAsync(UserAccountModel user)
        ***REMOVED***
            var email = await _userManager.GetEmailAsync(user);
            Email = email;

            Input = new InputModel
            ***REMOVED***
                NewEmail = email,
        ***REMOVED***;

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
    ***REMOVED***

        /// <summary>
        /// Handle the Get Request during the Email change process.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Handle the Post Request during the Email change process.
        /// Tries to change the email if the new email is different from the old one.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Tries to create and request the EmailSender to send an email to the user containing the account confirmation link after the email is successfully changed.
        /// </summary>
        /// <returns></returns>
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
