// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NutriFitWeb.Models;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
***REMOVED***
    /// <summary>
    /// EnableAuthenticatorModel class, derived from PageModel.
    /// </summary>
    public class EnableAuthenticatorModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly ILogger<EnableAuthenticatorModel> _logger;
        private readonly UrlEncoder _urlEncoder;

        private const string AuthenticatorUriFormat = "otpauth://totp/***REMOVED***0***REMOVED***:***REMOVED***1***REMOVED***?secret=***REMOVED***2***REMOVED***&issuer=***REMOVED***0***REMOVED***&digits=6";

        /// <summary>
        /// Build the EnableAuthenticatorModel model to be used when the user wants to enable 2FA in the account profile.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="logger">A generic interface for logging where the category name is derived from this class.</param>
        /// <param name="urlEncoder">Represents a URL character encoding.</param>
        public EnableAuthenticatorModel(
            UserManager<UserAccountModel> userManager,
            ILogger<EnableAuthenticatorModel> logger,
            UrlEncoder urlEncoder)
        ***REMOVED***
            _userManager = userManager;
            _logger = logger;
            _urlEncoder = urlEncoder;
    ***REMOVED***

        /// <summary>
        ///     Gets or sets the SharedKey
        /// </summary>
        public string SharedKey ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets the AuthenticatorUri
        /// </summary>
        public string AuthenticatorUri ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets a temporary string array with the RecoveryCodes
        /// </summary>
        [TempData]
        public string[] RecoveryCodes ***REMOVED*** get; set; ***REMOVED***

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
            ///     Gets or sets the verification code inputed by the user.
            /// </summary>
            [Required]
            [StringLength(7, ErrorMessage = "The ***REMOVED***0***REMOVED*** must be at least ***REMOVED***2***REMOVED*** and at max ***REMOVED***1***REMOVED*** characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Verification Code")]
            public string Code ***REMOVED*** get; set; ***REMOVED***
    ***REMOVED***

        /// <summary>
        /// Handle the Get Request during the 2FA setup.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync()
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            await LoadSharedKeyAndQrCodeUriAsync(user);

            return Page();
    ***REMOVED***

        /// <summary>
        /// Handle the Post Request during the 2FA setup.
        /// Tries to setup the 2FA and proceeds to redirect to the page responsible for showing the recovery codes.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                return NotFound($"Unable to load user with ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            if (!ModelState.IsValid)
            ***REMOVED***
                await LoadSharedKeyAndQrCodeUriAsync(user);
                return Page();
        ***REMOVED***

            // Strip spaces and hyphens
            var verificationCode = Input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            ***REMOVED***
                ModelState.AddModelError("Input.Code", "Verification code is invalid.");
                await LoadSharedKeyAndQrCodeUriAsync(user);
                return Page();
        ***REMOVED***

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            var userId = await _userManager.GetUserIdAsync(user);
            _logger.LogInformation("User with ID '***REMOVED***UserId***REMOVED***' has enabled 2FA with an authenticator app.", userId);

            StatusMessage = "Your authenticator app has been verified.";

            if (await _userManager.CountRecoveryCodesAsync(user) == 0)
            ***REMOVED***
                var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                RecoveryCodes = recoveryCodes.ToArray();
                return RedirectToPage("./ShowRecoveryCodes");
        ***REMOVED***
            else
            ***REMOVED***
                return RedirectToPage("./TwoFactorAuthentication");
        ***REMOVED***
    ***REMOVED***

        private async Task LoadSharedKeyAndQrCodeUriAsync(UserAccountModel user)
        ***REMOVED***
            // Load the authenticator key & QR code URI to display on the form
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            ***REMOVED***
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
        ***REMOVED***

            SharedKey = FormatKey(unformattedKey);

            var email = await _userManager.GetEmailAsync(user);
            AuthenticatorUri = GenerateQrCodeUri(email, unformattedKey);
    ***REMOVED***

        private string FormatKey(string unformattedKey)
        ***REMOVED***
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            ***REMOVED***
                result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');
                currentPosition += 4;
        ***REMOVED***
            if (currentPosition < unformattedKey.Length)
            ***REMOVED***
                result.Append(unformattedKey.AsSpan(currentPosition));
        ***REMOVED***

            return result.ToString().ToLowerInvariant();
    ***REMOVED***

        private string GenerateQrCodeUri(string email, string unformattedKey)
        ***REMOVED***
            return string.Format(
                CultureInfo.InvariantCulture,
                AuthenticatorUriFormat,
                _urlEncoder.Encode("Microsoft.AspNetCore.Identity.UI"),
                _urlEncoder.Encode(email),
                unformattedKey);
    ***REMOVED***
***REMOVED***
***REMOVED***
