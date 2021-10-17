using System;

namespace LogChugger.Import
{
    class DelayImportSchedulerSettings
    {
        public const string SectionName = "DelayImportScheduler";
        public TimeSpan ImportDelay { get; set; }
    }
}
