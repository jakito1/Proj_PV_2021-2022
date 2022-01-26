using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace NutriFitWeb.Services
***REMOVED***
    /// <summary>
    /// EmailSender class, derives from IEmailSender
    /// </summary>
    public class EmailSender : IEmailSender
    ***REMOVED***
        /// <summary>
        /// Build the EmailSender to be used when an email need to be sent to the user.
        /// </summary>
        /// <param name="optionsAccessor">EmailSender options</param>
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        ***REMOVED***
            Options = optionsAccessor.Value;
    ***REMOVED***

        /// <summary>
        /// Gets or sets the EmailSender Options.
        /// </summary>
        public AuthMessageSenderOptions Options ***REMOVED*** get; ***REMOVED*** //set only via Secret Manager

        /// <summary>
        /// Tries to send an email to the user.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <param name="subject">The message subject.</param>
        /// <param name="message">The message body.</param>
        /// <returns></returns>
        public Task SendEmailAsync(string email, string subject, string message)
        ***REMOVED***
            return Execute(Options.SendGridKey, subject, message, email);
    ***REMOVED***

        private Task Execute(string apiKey, string subject, string message, string email)
        ***REMOVED***
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            ***REMOVED***
                From = new EmailAddress("nutrifit.web@protonmail.com", "NutriFit"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
        ***REMOVED***;
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
    ***REMOVED***
***REMOVED***
***REMOVED***
