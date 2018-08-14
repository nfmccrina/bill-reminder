using System;
using System.Collections.Generic;
using System.Linq;
using BillReminderService.Service.Interfaces;
using BillReminderService.Service.Models;
using Newtonsoft.Json;
using Serilog;

namespace BillReminderService.Service
{
    public class BillJsonParser : IBillParser
    {
        public IEnumerable<Bill> ParseBillList(string billList)
        {
            IEnumerable<Bill> bills = null;

            try
            {
                bills = JsonConvert.DeserializeObject<IEnumerable<Bill>>(billList);
            }
            catch (JsonSerializationException ex)
            {
                Log.Error("Bill data is invalid and could not be read.");
                throw ex;
            }

            if (bills == null)
            {
                Log.Error("Bill data file is empty.");
                throw new ArgumentException();
            }

            Log.Debug(string.Format("Parser found {0} bills.", bills.Count()));

            return bills;
        }
    }
}