using System;

namespace LogChugger.Import
{
    class DelayImportSchedulerSettings
    {
        public const string SectionName = "delayImportScheduler";
        public TimeSpan ImportDelay { get; set; }
    }
}
