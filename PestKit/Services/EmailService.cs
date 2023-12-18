using System.Net.Mail;
using System.Net;
using PestKit.Interfaces;

namespace PestKit.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string emailTo, string subject, string body, bool IsHtml = false)
        {
            SmtpClient smtp = new SmtpClient(_configuration["Email:Host"], Convert.ToInt32(_configuration["Email:Port"]));
            smtp.EnableSsl = true; // Bunu elemiyende ya mail gondere bilmirdik, yada tehlukesiz sayilmirdi
            smtp.Credentials = new NetworkCredential(_configuration["Email:LoginEmail"], _configuration["Email:Password"]); // Bu klass login ve password gondermeye komek edir

            MailAddress from = new MailAddress(_configuration["Email:LoginEmail"], "PestKit Administration");

            MailAddress to = new MailAddress(emailTo);

            MailMessage message = new MailMessage(from, to);

            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = IsHtml;

            await smtp.SendMailAsync(message);
        }
    }
}
