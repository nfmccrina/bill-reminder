using System;
using System.Collections.Generic;
using System.Linq;
using BillReminderService.Service.Interfaces;
using BillReminderService.Service.Models;

namespace BillReminderService.Service
{
    public class BillDueCalculator : IBillDueCalculator
    {
        public BillDueResult IsBillDue(Bill bill)
        {
            BillDueResult result = new BillDueResult()
            {
                IsBillDue = false,
                ReminderMessage = ""
            };

            IEnumerable<int> reminders = bill.ReminderMessage
                .Split(';')
                .Where(s => int.TryParse(s, out var i))
                .Select(s => int.Parse(s))
                .Where(i => i > 0 && i <= 31)
                .OrderByDescending(i => i);

            foreach (int reminder in reminders)
            {
                if (isReminderNeeded(bill.DayOfMonth, reminder))
                {
                    result.IsBillDue = true;
                    result.ReminderMessage = string.Format("Reminder: {0} payment is due in {1} days.", bill.Name, reminder);
                    break;
                }
            }

            return result;
        }

        private bool isReminderNeeded(int day, int reminder)
        {
            return day - DateTime.Now.Day == reminder;
        }
    }
}