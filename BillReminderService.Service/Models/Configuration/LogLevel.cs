using System;

namespace BillReminderService.Service.Models.Configuration
{
    public class LogLevel
    {

        public static readonly Guid Debug = Guid.Parse("327299f2-652d-4583-b6a7-1277c5b996f6");
        public static readonly Guid Info = Guid.Parse("d933cd69-2d0e-4938-aead-ce754c0fa433");
        public static readonly Guid Warn = Guid.Parse("d166609a-6088-4651-ba20-84edef39cc98");
        public static readonly Guid Error = Guid.Parse("abcbc8de-a97f-495e-ae95-3dcfd9b5fc6e");
    }
}