using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillReminderService.Service.Interfaces;
using BillReminderService.Service.Models;

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
            foreach (BillDueResult billDueResult in _billParser
                .ParseBillList(billData)
                .Select(b => _billDueCalculator.IsBillDue(b))
                .Where(result => result.IsBillDue))
            {
                foreach (INotifier output in _notificationOutputs)
                {
                    await output.sendNotification(billDueResult.ReminderMessage);
                }
            }
        }

        private IBillParser _billParser;
        private IBillDueCalculator _billDueCalculator;
        private IEnumerable<INotifier> _notificationOutputs;
    }
}
