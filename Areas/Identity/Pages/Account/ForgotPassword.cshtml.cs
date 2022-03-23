// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
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
    /// ForgotPasswordModel class, derived from PageModel.
    /// </summary>
    public class ForgotPasswordModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Build the ForgotPasswordModel model to be used when the user forgets the password.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="emailSender">Microsoft EmailSender interface.</param>
        public ForgotPasswordModel(UserManager<UserAccountModel> userManager, IEmailSender emailSender)
        ***REMOVED***
            _userManager = userManager;
            _emailSender = emailSender;
    ***REMOVED***

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
            /// Gets or sets the Email inputed by the user.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email ***REMOVED*** get; set; ***REMOVED***
    ***REMOVED***

        /// <summary>
        /// Handles the Post Request during the ForgotPassword process.
        /// Send link to the user email with a password reset link.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user is null || !(await _userManager.IsEmailConfirmedAsync(user)))
                ***REMOVED***
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
            ***REMOVED***

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new ***REMOVED*** area = "Identity", code ***REMOVED***,
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='***REMOVED***HtmlEncoder.Default.Encode(callbackUrl)***REMOVED***'>clicking here</a>.");

                return RedirectToPage("./ForgotPasswordConfirmation");
        ***REMOVED***

            return Page();
    ***REMOVED***
***REMOVED***
***REMOVED***
