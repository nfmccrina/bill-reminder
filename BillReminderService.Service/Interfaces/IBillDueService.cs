using System.Threading.Tasks;

namespace BillReminderService.Service.Interfaces
{
    public interface IBillDueService
    {
        Task ProcessBills(string billData);
    }
}