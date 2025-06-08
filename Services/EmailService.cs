using DoDo.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using DoDo.Interfaces;
using System.Threading.Tasks;

namespace DoDo.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _settings;

        public EmailService(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mail = new MailMessage();
            // Usar nuevos campos para el remitente
            mail.From = new MailAddress(_settings.SenderEmail, _settings.SenderName);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            using var smtp = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.UserName, _settings.Password),
                EnableSsl = true, // Fuerza SSL (necesario para Gmail)
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Timeout = 10000 // 10 segundos timeout
            };

            await smtp.SendMailAsync(mail);
        }
    }
}