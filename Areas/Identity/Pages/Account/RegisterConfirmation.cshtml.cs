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
using System.Text;

namespace NutriFitWeb.Areas.Identity.Pages.Account
{
    /// <summary>
    /// RegisterConfirmationModel class, derived from PageModel.
    /// </summary>
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IEmailSender _sender;

        /// <summary>
        /// Build the RegisterConfirmationModel model to be used after the user registers and has to confirm the account.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="sender">Microsoft EmailSender interface.</param>
        public RegisterConfirmationModel(UserManager<UserAccountModel> userManager, IEmailSender sender)
        {
            _userManager = userManager;
            _sender = sender;
        }

        /// <summary>
        /// Gets or sets the Email inputed by the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Gets or sets the DisplayConfirmAccountLink (link to confirm the user account)
        /// </summary>
        public bool DisplayConfirmAccountLink { get; set; }

        /// <summary>
        ///     Gets or sets the EmailConfirmationUrl (link to confirm the user account sent to the email)
        /// </summary>
        public string EmailConfirmationUrl { get; set; }

        /// <summary>
        /// Handle the Get Request during the RegisterConfirmation process.
        /// Will get create and request the EmailSender to send an email to the user containing the account confirmation link.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email is null)
            {
                return RedirectToPage("/Index");
            }
            returnUrl = returnUrl ?? Url.Content("~/");
            UserAccountModel user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;
            // Once you add a real email sender, you should remove this code that lets you confirm the account
            DisplayConfirmAccountLink = false;
            if (DisplayConfirmAccountLink)
            {
                string userId = await _userManager.GetUserIdAsync(user);
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);
            }

            return Page();
        }
    }
}
