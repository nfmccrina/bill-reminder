using System.Threading.Tasks;

namespace BillReminderService.Service.Interfaces
{
    public interface INotifier
    {
        Task sendNotification(string message);
    }
}