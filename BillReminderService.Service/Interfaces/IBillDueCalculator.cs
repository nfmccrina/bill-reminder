using System;
using BillReminderService.Service.Models;

namespace BillReminderService.Service.Interfaces
{
    public interface IBillDueCalculator
    {
        BillDueResult IsBillDue(Bill bill, DateTime currentDate);
    }
}