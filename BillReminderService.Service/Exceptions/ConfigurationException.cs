using System;

namespace BillReminderService.Service.Exceptions
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message)
            : base(message)
        {
            
        }
    }
}