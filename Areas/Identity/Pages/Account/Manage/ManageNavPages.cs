// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Mvc.Rendering;

namespace NutriFitWeb.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    ///     ManageNavPages class.
    /// </summary>
    public static class ManageNavPages
    {
        /// <summary>
        ///     Gets the Index page name.
        /// </summary>
        public static string Index => "Index";

        /// <summary>
        ///     Gets the Email page name.
        /// </summary>
        public static string Email => "Email";

        /// <summary>
        ///     Gets the ChangePassword page name.
        /// </summary>
        public static string ChangePassword => "ChangePassword";

        /// <summary>
        ///     Gets the DownloadPersonalData page name.
        /// </summary>
        public static string DownloadPersonalData => "DownloadPersonalData";

        /// <summary>
        ///     Gets the DeletePersonalData page name.
        /// </summary>
        public static string DeletePersonalData => "DeletePersonalData";

        /// <summary>
        ///     Gets the ExternalLogins page name.
        /// </summary>
        public static string ExternalLogins => "ExternalLogins";

        /// <summary>
        ///     Gets the PersonalData page name.
        /// </summary>
        public static string PersonalData => "PersonalData";

        /// <summary>
        ///     Gets the TwoFactorAuthentication page name.
        /// </summary>
        public static string TwoFactorAuthentication => "TwoFactorAuthentication";

        /// <summary>
        /// Tries to create the Index page.
        /// </summary>
        /// <param name="viewContext">Context for the view execution.</param>
        /// <returns></returns>
        public static string IndexNavClass(ViewContext viewContext)
        {
            return PageNavClass(viewContext, Index);
        }

        /// <summary>
        /// Tries to create the Email page.
        /// </summary>
        /// <param name="viewContext">Context for the view execution.</param>
        /// <returns></returns>
        public static string EmailNavClass(ViewContext viewContext)
        {
            return PageNavClass(viewContext, Email);
        }

        /// <summary>
        /// Tries to create the ChangePassword page.
        /// </summary>
        /// <param name="viewContext">Context for the view execution.</param>
        /// <returns></returns>
        public static string ChangePasswordNavClass(ViewContext viewContext)
        {
            return PageNavClass(viewContext, ChangePassword);
        }

        /// <summary>
        /// Tries to create the DownloadPersonalData page.
        /// </summary>
        /// <param name="viewContext">Context for the view execution.</param>
        /// <returns></returns>
        public static string DownloadPersonalDataNavClass(ViewContext viewContext)
        {
            return PageNavClass(viewContext, DownloadPersonalData);
        }

        /// <summary>
        /// Tries to create the DeletePersonalData page.
        /// </summary>
        /// <param name="viewContext">Context for the view execution.</param>
        /// <returns></returns>
        public static string DeletePersonalDataNavClass(ViewContext viewContext)
        {
            return PageNavClass(viewContext, DeletePersonalData);
        }

        /// <summary>
        /// Tries to create the ExternalLogins page.
        /// </summary>
        /// <param name="viewContext">Context for the view execution.</param>
        /// <returns></returns>
        public static string ExternalLoginsNavClass(ViewContext viewContext)
        {
            return PageNavClass(viewContext, ExternalLogins);
        }

        /// <summary>
        /// Tries to create the PersonalData page.
        /// </summary>
        /// <param name="viewContext">Context for the view execution.</param>
        /// <returns></returns>
        public static string PersonalDataNavClass(ViewContext viewContext)
        {
            return PageNavClass(viewContext, PersonalData);
        }

        /// <summary>
        /// Tries to create the TwoFactorAuthentication page.
        /// </summary>
        /// <param name="viewContext">Context for the view execution.</param>
        /// <returns></returns>
        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext)
        {
            return PageNavClass(viewContext, TwoFactorAuthentication);
        }

        /// <summary>
        /// Tries to create the requested page.
        /// </summary>
        /// <param name="viewContext">Context for the view execution.</param>
        /// <param name="page">The name of the page to render.</param>
        /// <returns></returns>
        public static string PageNavClass(ViewContext viewContext, string page)
        {
            string activePage = viewContext.ViewData["ActivePage"] as string
                ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
