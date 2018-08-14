using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillReminderService.Service.Interfaces;
using BillReminderService.Service.Models.Configuration;
using MailKit.Net.Smtp;
using MimeKit;

namespace BillReminderService
{
    public class EmailNotifier : INotifier
    {
        public EmailNotifier(EmailNotificationSettings settings)
        {
            _settings = settings;
        }

        public async Task sendNotification(string message)
        {
            using (var client = new SmtpClient())
            {
                client.Connect(_settings.SMTPHost, _settings.SMTPPort, false);
                client.Authenticate(_settings.Credentials);
                await client.SendAsync(BuildMailMessage(message));
                await client.DisconnectAsync(true);
                    
            }
        }

        private MimeMessage BuildMailMessage(string text)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromAddress.Item1, _settings.FromAddress.Item2));
            message.To.AddRange(_settings.ToAddresses.Select(t => new MailboxAddress(t.Item1, t.Item2)));
            message.Subject = "BillReminderService: Bill Due Reminder";
            message.Body = new TextPart("plain")
            {
                Text = text
            };

            return message;
        }
        
        private EmailNotificationSettings _settings;
    }
}