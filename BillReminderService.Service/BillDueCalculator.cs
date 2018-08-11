using System;
using System.Collections.Generic;
using System.Linq;
using BillReminderService.Service.Interfaces;
using BillReminderService.Service.Models;

namespace BillReminderService.Service
{
    public class BillDueCalculator : IBillDueCalculator
    {
        public BillDueResult IsBillDue(Bill bill, DateTime currentDate)
        {
            BillDueResult result = new BillDueResult()
            {
                IsBillDue = false,
                ReminderMessage = ""
            };

            IEnumerable<int> reminders = bill
                .ReminderIntervals
                .Where(i => i > 0 && i <= 31)
                .OrderByDescending(i => i);

            foreach (int reminder in reminders)
            {
                if (isReminderNeeded(bill.DayOfMonth, reminder, currentDate))
                {
                    result.IsBillDue = true;
                    result.ReminderMessage = string.Format("Reminder: {0} payment is due in {1} days.", bill.Name, reminder);
                    break;
                }
            }

            return result;
        }

        private bool isReminderNeeded(int day, int reminder, DateTime currentDate)
        {
            return day - currentDate.Day == reminder;
        }
    }
}