

using BillReminderService.Service.Interfaces;
using BillReminderService.Service.Models.Configuration;
using Microsoft.Extensions.Configuration;

namespace BillReminderService.Service
{
    public class ServiceSettingsBuilder : IServiceSettingsBuilder
    {
        public BillReminderServiceSettings GetSettings(IConfiguration configuration)
        {
            return new BillReminderServiceSettings()
                .AddNotificationSinks(configuration)
                .AddDataFilePath(configuration)
                .AddEmailSettings(configuration);
        }
    }
}