// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Areas.Identity.Pages.Account
***REMOVED***
    /// <summary>
    /// RegisterModel class, derived from PageModel.
    /// </summary>
    public class RegisterModel : PageModel
    ***REMOVED***
        private readonly SignInManager<UserAccountModel> _signInManager;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IUserStore<UserAccountModel> _userStore;
        private readonly IUserEmailStore<UserAccountModel> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Build the RegisterModel model to be used when the user decides to register.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="userStore">Provides an abstraction for a store which manages the UserAccountModel.</param>
        /// <param name="signInManager">Provides the APIs for user sign in using the UserAccountModel.</param>
        /// <param name="logger">A generic interface for logging where the category name is derived from this class.</param>
        /// <param name="emailSender">Microsoft EmailSender interface.</param>
        public RegisterModel(
            UserManager<UserAccountModel> userManager,
            IUserStore<UserAccountModel> userStore,
            SignInManager<UserAccountModel> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        ***REMOVED***
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
    ***REMOVED***

        /// <summary>
        /// Gets or sets the data containing the user input.
        /// </summary>
        [BindProperty]
        public InputModel Input ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets the temporary string containing the ErrorMessage.
        /// </summary>
        public string ReturnUrl ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Gets or sets a list with the ExternalLogins.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        ///     Inner class specifying what data the user can input.
        /// </summary>
        public class InputModel
        ***REMOVED***
            /// <summary>
            /// Gets or sets the UserName inputed by the user.
            /// </summary>
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "User Name")]
            public string UserName ***REMOVED*** get; set; ***REMOVED***

            /// <summary>
            /// Gets or sets the Email inputed by the user.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email ***REMOVED*** get; set; ***REMOVED***

            /// <summary>
            ///     Gets or sets the Password inputed by the user.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The ***REMOVED***0***REMOVED*** must be at least ***REMOVED***2***REMOVED*** and at max ***REMOVED***1***REMOVED*** characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password ***REMOVED*** get; set; ***REMOVED***

            /// <summary>
            ///     Gets or sets the repeated Password inputed by the user.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirmar password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword ***REMOVED*** get; set; ***REMOVED***

        
    ***REMOVED***

        /// <summary>
        /// Handles the Get Request during the register process.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task OnGetAsync(string returnUrl = null)
        ***REMOVED***
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    ***REMOVED***

        /// <summary>
        /// Handles the Post Request during the register process.
        /// Tries to create a user account.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        ***REMOVED***
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            ***REMOVED***
                var user = CreateUser();

                string accountType = Request.Form["AccountType"].ToString();
                if (string.IsNullOrEmpty(accountType)) ***REMOVED*** accountType = "client"; ***REMOVED***

                await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                ***REMOVED***
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new ***REMOVED*** area = "Identity", userId = userId, code = code, returnUrl = returnUrl ***REMOVED***,
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='***REMOVED***HtmlEncoder.Default.Encode(callbackUrl)***REMOVED***'>clicking here</a>.");

                    if (accountType.Equals("client")) ***REMOVED***
                        await _context.Client.AddAsync(new() ***REMOVED*** UserAccountModel = user***REMOVED***);
                ***REMOVED*** else if (accountType.Equals("trainer"))
                    ***REMOVED***
                        //toDo
                ***REMOVED*** else if (accountType.Equals("nutritionist"))
                    ***REMOVED***
                        //toDo
                ***REMOVED***else if (accountType.Equals("gym"))
                    ***REMOVED***
                        await _context.Gym.AddAsync(new() ***REMOVED*** UserAccount = user ***REMOVED***);
                ***REMOVED***

                    await _context.SaveChangesAsync();

                    await _userManager.AddToRoleAsync(user, accountType);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    ***REMOVED***
                        return RedirectToPage("RegisterConfirmation", new ***REMOVED*** email = Input.Email, returnUrl = returnUrl ***REMOVED***);
                ***REMOVED***
                    else
                    ***REMOVED***
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                ***REMOVED***
            ***REMOVED***
                foreach (var error in result.Errors)
                ***REMOVED***
                    ModelState.AddModelError(string.Empty, error.Description);
            ***REMOVED***
        ***REMOVED***

            // If we got this far, something failed, redisplay form
            return Page();
    ***REMOVED***

        private UserAccountModel CreateUser()
        ***REMOVED***
            try
            ***REMOVED***
                return new UserAccountModel();
        ***REMOVED***
            catch
            ***REMOVED***
                throw new InvalidOperationException($"Can't create an instance of '***REMOVED***nameof(UserAccountModel)***REMOVED***'. " +
                    $"Ensure that '***REMOVED***nameof(UserAccountModel)***REMOVED***' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        ***REMOVED***
    ***REMOVED***

        private IUserEmailStore<UserAccountModel> GetEmailStore()
        ***REMOVED***
            if (!_userManager.SupportsUserEmail)
            ***REMOVED***
                throw new NotSupportedException("The default UI requires a user store with email support.");
        ***REMOVED***
            return (IUserEmailStore<UserAccountModel>)_userStore;
    ***REMOVED***
***REMOVED***
***REMOVED***
