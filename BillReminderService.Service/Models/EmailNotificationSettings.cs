using System.Collections.Generic;

namespace BillReminderService.Service.Models
{
    public class EmailNotificationSettings
    {
        public string SMTPHost { get; set; }
        public int SMTPPort { get; set; }
        public string FromAddress { get; set; }
        public IEnumerable<string> ToAddresses { get; set; }
    }
}