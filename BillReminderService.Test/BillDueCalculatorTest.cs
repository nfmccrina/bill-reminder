using System;
using Xunit;
using BillReminderService.Service;
using BillReminderService.Service.Models;
using System.Collections.Generic;

namespace BillReminderService.Test
{
    public class BillDueCalculatorTest
    {
        [Fact]
        public void BillDueCalculator_SingleReminder_ReminderMatch_ShouldReportBillDue()
        {
            Bill testData = new Bill();

            testData.Name = "Test Bill 1";
            testData.DayOfMonth = 2;
            testData.ReminderIntervals = new List<int>() { 1 };

            BillDueCalculator sut = new BillDueCalculator();

            BillDueResult result = sut.IsBillDue(testData, new DateTime(2018, 1, 1));

            Assert.True(result.IsBillDue);
            Assert.Equal(string.Format("Reminder: {0} payment is due in {1} days.", testData.Name, 1), result.ReminderMessage);
        }

        [Fact]
        public void BillDueCalculator_SingleReminder_NoReminderMatch_ShouldNotReportBillDue()
        {
            Bill testData = new Bill();

            testData.Name = "Test Bill 1";
            testData.DayOfMonth = 3;
            testData.ReminderIntervals = new List<int>() { 1 };

            BillDueCalculator sut = new BillDueCalculator();

            BillDueResult result = sut.IsBillDue(testData, new DateTime(2018, 1, 1));

            Assert.False(result.IsBillDue);
        }
    }
}
