using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BillReminderService.Service.Interfaces;
using BillReminderService.Service.Models;
using Serilog;

namespace BillReminderService.Service
{
    public class BillDueCalculator : IBillDueCalculator
    {
        public BillDueResult IsBillDue(Bill bill, DateTime currentDate)
        {
            StringBuilder logMessage = new StringBuilder();
            BillDueResult result = new BillDueResult()
            {
                IsBillDue = false,
                ReminderMessage = ""
            };

            logMessage.AppendFormat("Checking if {0} is due...", bill.Name);

            IEnumerable<int> reminders = bill
                .ReminderIntervals?
                .Where(i => i > 0 && i <= 31)
                .OrderByDescending(i => i);
            
            if (reminders == null)
            {
                Log.Information(string.Format("No reminder intervals specified for {0}. Skipping this bill.", bill.Name));
            }

            foreach (int reminder in reminders)
            {
                if (isReminderNeeded(bill.DayOfMonth, reminder, currentDate))
                {
                    result.IsBillDue = true;
                    result.ReminderMessage = string.Format("{0}: payment is due in {1} days.", bill.Name, reminder);
                    break;
                }
            }

            logMessage.Append(result.IsBillDue ? "yes" : "no");

            Log.Debug(logMessage.ToString());

            return result;
        }

        private bool isReminderNeeded(int day, int reminder, DateTime currentDate)
        {
            return day - currentDate.Day == reminder;
        }
    }
}