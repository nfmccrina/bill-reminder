using System;
using System.Collections.Generic;
using System.Net;

namespace BillReminderService.Service.Models.Configuration
{
    public class EmailNotificationSettings
    {
        public ICredentials Credentials { get; set; }
        public string SMTPHost { get; set; }
        public int SMTPPort { get; set; }
        public Tuple<string, string> FromAddress { get; set; }
        public IEnumerable<Tuple<string, string>> ToAddresses { get; set; }
    }
}