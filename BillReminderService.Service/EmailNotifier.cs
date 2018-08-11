using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BillReminderService.Service.Interfaces;
using BillReminderService.Service.Models;

namespace BillReminderService
{
    public class EmailNotifier : INotifier
    {
        public EmailNotifier(ICredentialsByHost credentials, EmailNotificationSettings settings)
        {
            _credentials = credentials;
            _settings = settings;
        }

        public async Task sendNotification(string message)
        {
            SmtpClient client = BuildEmailClient();

            await client.SendMailAsync(BuildMailMessage(message));
        }

        private SmtpClient BuildEmailClient()
        {
            SmtpClient client = new SmtpClient(_settings.SMTPHost, _settings.SMTPPort);
            
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = _credentials; //new NetworkCredential("billremindersvc@gmail.com", "4H4wn62V");

            return client;
        }

        private MailMessage BuildMailMessage(string text)
        {
            MailMessage message = new MailMessage();
            
            foreach (var addr in _settings.ToAddresses.Select(addr => new MailAddress(addr)))
            {
                message.To.Add(addr);
            }

            message.Sender = new MailAddress(_settings.FromAddress);
            message.Subject = "BillReminderService: Bill Due Reminder";
            message.Body = text;

            return message;
        }

        private ICredentialsByHost _credentials;
        private EmailNotificationSettings _settings;
    }
}