using ExpenseManagement.Application.Services.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using MimeKit;
using MailKit.Security;

namespace ExpenseManagement.Application.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SMTPServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SMTPPort"]);
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var senderPassword = _configuration["EmailSettings:SenderPassword"];

                var client = new SmtpClient(smtpServer)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true,
                    Timeout = 30000
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                toEmail = "mounika.chittemsetty@gmail.com";
                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public async Task SendEmailWithAttachment(byte[] file, string fileName)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SMTPServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SMTPPort"]);
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var senderPassword = _configuration["EmailSettings:SenderPassword"];

                var client = new MailKit.Net.Smtp.SmtpClient(); // Use MailKit's SmtpClient

                var message = new MimeMessage { Subject = "Expense Report" };
                message.From.Add(new MailboxAddress("Expense Manager", "supraja.tangella@gmail.com"));
                message.To.Add(new MailboxAddress("", "mounika.chittemsetty@gmail.com"));
                var body = new TextPart("plain") { Text = "Please find your expense report attached." };
                var attachment = new MimePart("application", "pdf") { Content = new MimeContent(new MemoryStream(file)), FileName = fileName };
                message.Body = new MimeKit.Multipart("mixed") { body, attachment };

                await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email with attachment: {ex.Message}");
            }
        }
    }
}
