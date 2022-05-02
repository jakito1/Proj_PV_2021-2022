// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NutriFitWeb.Models;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
{
    //[Authorize(Roles = "client")]
    /// <summary>
    /// PersonalDataModel class, derived from PageModel.
    /// </summary>
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<UserAccountModel> _userManager;

        /// <summary>
        /// Build the PersonalDataModel model to be used when the user wants to view the page where it's possible to download the personal data from the account profile.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="logger">A generic interface for logging where the category name is derived from this class.</param>
        public PersonalDataModel(
            UserManager<UserAccountModel> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Handle the Get Request during the page creation process.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGet()
        {
            UserAccountModel? user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }
    }
}
