using System.Collections.Generic;
using BillReminderService.Service.Interfaces;
using BillReminderService.Service.Models;
using Newtonsoft.Json;

namespace BillReminderService.Service
{
    public class BillJsonParser : IBillParser
    {
        public IEnumerable<Bill> ParseBillList(string billList)
        {
            return JsonConvert.DeserializeObject<IEnumerable<Bill>>(billList);
        }
    }
}