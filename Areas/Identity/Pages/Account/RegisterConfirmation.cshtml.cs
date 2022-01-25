// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using NutriFitWeb.Models;

namespace NutriFitWeb.Areas.Identity.Pages.Account
***REMOVED***
    /// <summary>
    /// RegisterConfirmationModel class, derived from PageModel.
    /// </summary>
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IEmailSender _sender;

        /// <summary>
        /// Build the RegisterConfirmationModel model to be used after the user registers and has to confirm the account.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="sender">Microsoft EmailSender interface.</param>
        public RegisterConfirmationModel(UserManager<UserAccountModel> userManager, IEmailSender sender)
        ***REMOVED***
            _userManager = userManager;
            _sender = sender;
    ***REMOVED***

        /// <summary>
        /// Gets or sets the Email inputed by the user.
        /// </summary>
        public string Email ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets the DisplayConfirmAccountLink (link to confirm the user account)
        /// </summary>
        public bool DisplayConfirmAccountLink ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets the EmailConfirmationUrl (link to confirm the user account sent to the email)
        /// </summary>
        public string EmailConfirmationUrl ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Handle the Get Request during the RegisterConfirmation process.
        /// Will get create and request the EmailSender to send an email to the user containing the account confirmation link.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        ***REMOVED***
            if (email == null)
            ***REMOVED***
                return RedirectToPage("/Index");
        ***REMOVED***
            returnUrl = returnUrl ?? Url.Content("~/");
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            ***REMOVED***
                return NotFound($"Unable to load user with email '***REMOVED***email***REMOVED***'.");
        ***REMOVED***

            Email = email;
            // Once you add a real email sender, you should remove this code that lets you confirm the account
            DisplayConfirmAccountLink = false;
            if (DisplayConfirmAccountLink)
            ***REMOVED***
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new ***REMOVED*** area = "Identity", userId = userId, code = code, returnUrl = returnUrl ***REMOVED***,
                    protocol: Request.Scheme);
        ***REMOVED***

            return Page();
    ***REMOVED***
***REMOVED***
***REMOVED***
