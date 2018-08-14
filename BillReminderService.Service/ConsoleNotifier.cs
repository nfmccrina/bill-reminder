using System;
using System.Threading.Tasks;
using BillReminderService.Service.Interfaces;

namespace BillReminderService.Service
{
    public class ConsoleNotifier : INotifier
    {
        public Task sendNotification(string message)
        {
            Console.WriteLine(message);
            
            return Task.CompletedTask;
        }
    }
}