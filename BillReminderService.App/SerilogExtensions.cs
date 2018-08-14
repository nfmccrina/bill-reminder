using System;
using BillReminderService.Service.Models.Configuration;
using Serilog;

namespace BillReminderService.App
{
    public static class SerilogExtensions
    {
        public static LoggerConfiguration SetMinimumLevel(this LoggerConfiguration loggerConfiguration, Guid? logLevel)
        {
            if (logLevel.HasValue)
            {
                if (logLevel.Value == LogLevel.Debug)
                {
                    return loggerConfiguration.MinimumLevel.Debug();
                }
                else if (logLevel.Value == LogLevel.Info)
                {
                    return loggerConfiguration.MinimumLevel.Information();
                }
                else if (logLevel.Value == LogLevel.Warn)
                {
                    return loggerConfiguration.MinimumLevel.Warning();
                }
                else if (logLevel.Value == LogLevel.Error)
                {
                    return loggerConfiguration.MinimumLevel.Error();
                }
                else
                {
                    return loggerConfiguration.MinimumLevel.Error();
                }
            }
            else
            {
                return loggerConfiguration.MinimumLevel.Error();
            }
        }
    }
}