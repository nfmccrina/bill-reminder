using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillReminderService.Service.Interfaces;
using BillReminderService.Service.Models;
using Serilog;

namespace BillReminderService.Service
{
    public class BillDueService : IBillDueService
    {
        public BillDueService(
            IBillParser billParser,
            IBillDueCalculator billDueCalculator,
            IEnumerable<INotifier> notificationOutputs)
        {
            _billParser = billParser;
            _billDueCalculator = billDueCalculator;
            _notificationOutputs = notificationOutputs;
        }
        public async Task ProcessBills(string billData)
        {
            string message = _billParser
                .ParseBillList(billData)
                .Select(b => _billDueCalculator.IsBillDue(b, DateTime.Now))
                .Where(result => result.IsBillDue)
                .Aggregate(
                    "Hi, this is a reminder that the following bills are due:",
                    (valueSoFar, result) => string.Format("{0}{1}{2}{3}", valueSoFar, Environment.NewLine, Environment.NewLine, result.ReminderMessage)
                );
                
            foreach (INotifier output in _notificationOutputs)
            {
                await output.sendNotification(message);
            }
        }

        private IBillParser _billParser;
        private IBillDueCalculator _billDueCalculator;
        private IEnumerable<INotifier> _notificationOutputs;
    }
}
