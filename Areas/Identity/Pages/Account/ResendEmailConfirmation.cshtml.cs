// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using NutriFitWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace NutriFitWeb.Areas.Identity.Pages.Account
{
    /// <summary>
    /// ResendEmailConfirmationModel class, derived from PageModel.
    /// </summary>
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Build the ResendEmailConfirmationModel model to be used when the user requests a new link to confirm the account.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="emailSender">Microsoft EmailSender interface.</param>
        public ResendEmailConfirmationModel(UserManager<UserAccountModel> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

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
            /// Gets or sets the Email inputed by the user.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        /// <summary>
        /// Handles the Get Request during the ResendEmailConfirmation process.
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// Handles the Post Request during the ResendEmailConfirmation process.
        /// Tries to create and request the EmailSender to send an email to the user containing the account confirmation link.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            UserAccountModel user = await _userManager.FindByEmailAsync(Input.Email);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
                return Page();
            }

            string userId = await _userManager.GetUserIdAsync(user);
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                Input.Email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
            return Page();
        }
    }
}
