using Insurance.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Insurance.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration config;

        public EmailService(IConfiguration config)
        {
            this.config = config;
        }

        public async Task SendCredentialsAsync(
            string toEmail,
            string password,
            string role)
        {
            var host = config["Smtp:Host"];
            var port = int.Parse(config["Smtp:Port"]!);
            var user = config["Smtp:Username"];
            var pass = config["Smtp:Password"];
            var fromEmail = config["Smtp:FromEmail"];

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = true
            };

            using var message = new MailMessage
            {
                From = new MailAddress(fromEmail!),
                Subject = $"Insurance Application - {role} Credentials",
                IsBodyHtml = true,
                Body = $@"
                    <h2>Welcome to Insurance Application</h2>
                    <p><strong>Email:</strong> {toEmail}</p>
                    <p><strong>Password:</strong> {password}</p>
                    <p><strong>Role:</strong> {role}</p>"
            };

            message.To.Add(toEmail);

            await client.SendMailAsync(message);
        }
    }
}