using System;
using System.Collections.Generic;
using BillReminderService.Service.Models;

namespace BillReminderService.Service.Models.Configuration
{
    public class BillReminderServiceSettings
    {
        public Guid MinimumLogLevel { get; set; } 
        public string DataFilePath { get; set; }
        public IEnumerable<string> NotificationSinks { get; set; }
        public EmailNotificationSettings emailNotificationSettings { get; set; }
    }
}