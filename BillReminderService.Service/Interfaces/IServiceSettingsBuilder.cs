using BillReminderService.Service.Models.Configuration;
using Microsoft.Extensions.Configuration;

namespace BillReminderService.Service.Interfaces
{
    public interface IServiceSettingsBuilder
    {
        BillReminderServiceSettings GetSettings(IConfiguration configuration);
    }
}