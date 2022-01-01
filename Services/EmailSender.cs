using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace NutriFitWeb.Services
***REMOVED***
    public class EmailSender : IEmailSender
    ***REMOVED***
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        ***REMOVED***
            Options = optionsAccessor.Value;
    ***REMOVED***

        public AuthMessageSenderOptions Options ***REMOVED*** get; ***REMOVED*** //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        ***REMOVED***
            return Execute(Options.SendGridKey, subject, message, email);
    ***REMOVED***

        public Task Execute(string apiKey, string subject, string message, string email)
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
