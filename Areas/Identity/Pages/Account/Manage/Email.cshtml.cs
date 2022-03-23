// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using NutriFitWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// EmailModel class, derived from PageModel.
    /// </summary>
    public class EmailModel : PageModel
    {
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Build the EmailModel model to be used when the user wants to view the page where it's possible to change the email in the account profile.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="signInManager">Provides the APIs for user sign in using the UserAccountModel.</param>
        /// <param name="emailSender">Microsoft EmailSender interface.</param>
        public EmailModel(
            UserManager<UserAccountModel> userManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Gets or sets the user's Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Gets or sets the flag whether the email is confirmed.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        ///     Gets or sets the temporary string StatusMessage.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        /// Gets or sets the data containing the user input.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     Inner class specifying what data the user can input.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     Gets or sets the new email inputed by the user.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Novo Email")]
            public string NewEmail { get; set; }
        }

        private async Task LoadAsync(UserAccountModel user)
        {
            string email = await _userManager.GetEmailAsync(user);
            Email = email;

            Input = new InputModel
            {
                NewEmail = email,
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        /// <summary>
        /// Handle the Get Request during the Email change process.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync()
        {
            UserAccountModel user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        /// <summary>
        /// Handle the Post Request during the Email change process.
        /// Tries to change the email if the new email is different from the old one.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            UserAccountModel user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            string email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                string userId = await _userManager.GetUserIdAsync(user);
                string code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                string callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { area = "Identity", userId, email = Input.NewEmail, code },
                    protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    Input.NewEmail,
                    "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                StatusMessage = "Confirmation link to change email sent. Please check your email.";
                return RedirectToPage();
            }

            StatusMessage = "Your email is unchanged.";
            return RedirectToPage();
        }

        /// <summary>
        /// Tries to create and request the EmailSender to send an email to the user containing the account confirmation link after the email is successfully changed.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            UserAccountModel user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            string userId = await _userManager.GetUserIdAsync(user);
            string email = await _userManager.GetEmailAsync(user);
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId, code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
