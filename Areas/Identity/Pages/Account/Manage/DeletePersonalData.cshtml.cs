// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NutriFitWeb.Models;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
***REMOVED***
    /// <summary>
    /// DeletePersonalDataModel class, derived from PageModel.
    /// </summary>
    public class DeletePersonalDataModel : PageModel
    ***REMOVED***
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly SignInManager<UserAccountModel> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;

        /// <summary>
        /// Build the DeletePersonalDataModel model to be used when the user wants to view the page where it's possible to delete the account in the account profile.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="signInManager">Provides the APIs for user sign in using the UserAccountModel.</param>
        /// <param name="logger">A generic interface for logging where the category name is derived from this class.</param>
        public DeletePersonalDataModel(
            UserManager<UserAccountModel> userManager,
            SignInManager<UserAccountModel> signInManager,
            ILogger<DeletePersonalDataModel> logger)
        ***REMOVED***
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
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
            ///     Gets or sets the Password inputed by the user.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password ***REMOVED*** get; set; ***REMOVED***
    ***REMOVED***

        /// <summary>
        ///     Gets or sets the flag whether the user has or not password in the account.
        /// </summary>
        public bool RequirePassword ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Handle the Get Request during the DeletePersonalData process.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGet()
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                return NotFound($"Não foi possível carregar o utilizador com o ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
    ***REMOVED***

        /// <summary>
        /// Handle the Post Request during the DeletePersonalData process.
        /// Tries to delete the user account, provided the password given is correct.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Exception thrown when an error occurs during the account deletion</exception>
        public async Task<IActionResult> OnPostAsync()
        ***REMOVED***
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            ***REMOVED***
                return NotFound($"Não foi possível carregar o utilizador com o ID '***REMOVED***_userManager.GetUserId(User)***REMOVED***'.");
        ***REMOVED***

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            ***REMOVED***
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                ***REMOVED***
                    ModelState.AddModelError(string.Empty, "Palavra-passe incorreta.");
                    return Page();
            ***REMOVED***
        ***REMOVED***

            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            ***REMOVED***
                throw new InvalidOperationException($"Erro inesperado ao tentar apagar o utilizador.");
        ***REMOVED***

            await _signInManager.SignOutAsync();

            _logger.LogInformation("O utilizador com o ID '***REMOVED***UserId***REMOVED***' apagou a sua conta.", userId);

            return Redirect("~/");
    ***REMOVED***
***REMOVED***
***REMOVED***
