using System;
using System.Linq;
using System.Net;
using BillReminderService.Service.Exceptions;
using Microsoft.Extensions.Configuration;

namespace BillReminderService.Service.Models.Configuration
{
    public static class BillReminderServiceSettingsExtensions
    {
        public static BillReminderServiceSettings AddNotificationSinks(this BillReminderServiceSettings settings, IConfiguration configuration)
        {
            settings.NotificationSinks = configuration["notificationSinks"]?.Split(',').AsEnumerable() ?? throw new ConfigurationException("No notification sinks configured.");

            return settings;
        }

        public static BillReminderServiceSettings AddDataFilePath(this BillReminderServiceSettings settings, IConfiguration configuration)
        {
            settings.DataFilePath = configuration["dataFilePath"] ?? throw new ConfigurationException("Data file path is not configured.");

            return settings;
        }

        public static BillReminderServiceSettings AddLogLevel(this BillReminderServiceSettings settings, IConfiguration config)
        {
            if (config["logLevel"]?.ToLower() == "debug")
            {
                settings.MinimumLogLevel = LogLevel.Debug;
            }
            else if (config["loglevel"]?.ToLower() == "info")
            {
                settings.MinimumLogLevel = LogLevel.Info;
            }
            else if (config["loglevel"]?.ToLower() == "warn")
            {
                settings.MinimumLogLevel = LogLevel.Warn;
            }
            else if (config["loglevel"]?.ToLower() == "error")
            {
                settings.MinimumLogLevel = LogLevel.Error;
            }
            else
            {
                settings.MinimumLogLevel = Guid.Empty;
            }

            return settings;
        }

        public static BillReminderServiceSettings AddEmailSettings(this BillReminderServiceSettings settings, IConfiguration config)
        {
            string smtpUserName = config.GetSection("Email")["smtpUserName"] ?? throw new ConfigurationException("No SMTP email account configured.");
            string smtpPassword = config.GetSection("Email")["smtpPassword"] ?? throw new ConfigurationException("No SMTP password configured.");
            string smtpHost = config.GetSection("Email")["smtpHost"] ?? throw new ConfigurationException("No SMTP host configured.");
            string senderAddress = config.GetSection("Email")["senderAddress"] ?? throw new ConfigurationException("No email sender address configured.");
            string recipientAddresses = config.GetSection("Email")["recipientAddresses"] ?? throw new ConfigurationException("No email recipient addresses configured.");
            int smtpPort;

            settings.emailNotificationSettings = new EmailNotificationSettings()
            {
                SMTPHost = config.GetSection("Email")["smtpHost"] ?? throw new Exception("SMTP host not configured."),
                SMTPPort = int.TryParse(config.GetSection("Email")["smtpPort"], out smtpPort) ? smtpPort : throw new ConfigurationException("SMTP port not configured."),
                Credentials = new NetworkCredential(smtpUserName, smtpPassword, smtpHost),
                FromAddress = BuildEmailAddress(senderAddress),
                ToAddresses = recipientAddresses.Split(';').Select(s => BuildEmailAddress(s))
            };

            return settings;
        }

        private static Tuple<string, string> BuildEmailAddress(string email)
        {
            var parts = email.Split(',');

            if (parts.Length > 1)
            {
                return new Tuple<string, string>(parts[0], parts[1]);
            }
            else
            {
                return new Tuple<string, string>(parts[0], parts[0]);
            }
        }
    }
}