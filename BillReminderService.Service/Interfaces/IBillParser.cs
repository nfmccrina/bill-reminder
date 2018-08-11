using System.Collections.Generic;
using BillReminderService.Service.Models;

namespace BillReminderService.Service.Interfaces
{
    public interface IBillParser
    {
        IEnumerable<Bill> ParseBillList(string billList);
    }
}