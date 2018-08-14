using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using BillReminderService.Service;
using BillReminderService.Service.Interfaces;
using BillReminderService.Service.Models;
using BillReminderService.Service.Models.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;

namespace BillReminderService.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Program app = new Program();
            BillReminderServiceSettings settings = null;
            string json;

            try
            {
                settings = app.BuildConfiguration(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            IServiceProvider serviceProvider = app.ConfigureServices(settings);

            try
            {
                try
                {
                    json = app.GetBillData(settings.DataFilePath);
                }
                catch (FileNotFoundException ex)
                {
                    Log.Error(string.Format("Could not find data file. Looking for \"{0}\".", settings.DataFilePath));
                    throw ex;
                }

                serviceProvider.GetService<IBillDueService>().ProcessBills(json).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log.Debug(ex, ex.Message);
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private string GetBillData(string path)
        {
            return File.ReadAllText(path);
        }

        private IServiceProvider ConfigureServices(BillReminderServiceSettings settings)
        {
            var serviceCollection = new ServiceCollection();

            BuildLogger(settings);

            serviceCollection.AddSingleton<IBillParser, BillJsonParser>();
            serviceCollection.AddSingleton<IBillDueCalculator, BillDueCalculator>();
            serviceCollection.AddSingleton<ICredentialsByHost>(s => GetEmailCredentials(settings));
            serviceCollection.AddSingleton<IEnumerable<INotifier>>(s => GetNotifiers(s, settings));
            serviceCollection.AddSingleton<IBillDueService, BillDueService>();

            return serviceCollection.BuildServiceProvider();
        }

        private void BuildLogger(BillReminderServiceSettings settings)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .SetMinimumLevel(settings.MinimumLogLevel)
                .CreateLogger();
        }

        private ICredentialsByHost GetEmailCredentials(BillReminderServiceSettings settings)
        {
            CredentialCache cache = new CredentialCache();

            cache.Add("", 587, "Basic", new NetworkCredential());

            return cache;
        }

        private IEnumerable<INotifier> GetNotifiers(IServiceProvider serviceProvider, BillReminderServiceSettings settings)
        {
            var notifiers = new List<INotifier>();

            if (settings.NotificationSinks.Contains("console"))
            {
                notifiers.Add(new ConsoleNotifier());
            }

            if (settings.NotificationSinks.Contains("email"))
            {
                notifiers.Add(new EmailNotifier(settings.emailNotificationSettings));
            }

            return notifiers;
        }

        private BillReminderServiceSettings BuildConfiguration(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appSettings.json"), true)
                .AddCommandLine(args)
                .Build();

            return new ServiceSettingsBuilder().GetSettings(config);
        }
    }
}
