namespace EmailServices
{
    public class EmailService : IEmailService
    {   
        // method that is called and takes parameters to prepare to send off.  .wait() is required here because procedure
        // is run async, wait() will sync
        public void Send(IEnumerable<MailAddress> to, MailAddress from, string subject, string htmlContent, string htmlBody)
        {
            Execute(to, from, subject, htmlContent, htmlBody).Wait();
        }

        async Task Execute(IEnumerable<MailAddress> to, MailAddress from, string subject, string htmlBody, string htmlContent)
        {
            var apiKey = ConfigurationManager.AppSettings["somekey"];
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(from.Address, from.DisplayName),
                Subject = subject,
                HtmlContent = htmlContent
            };

            // loop through to, which can be any type of collection that holds email addresses
            foreach(var i in to)
            {
                msg.AddTo(new EmailAddress(i.Address, i.DisplayName));
            }

            // ConfigureAwait(false) is for fixing unnecessary thread context synchronization and blocks that will occur when running due to async/await
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

            if ((int)response.StatusCode >= 300)
            {
                var error = response.StatusCode;
                throw new Exception($"SendGrid Error Occured, email was not sent out.  SendGrid Error: '{error}'");
            }
        }
    }
}