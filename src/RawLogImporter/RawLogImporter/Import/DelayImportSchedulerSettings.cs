using System;

namespace LogChugger.Import
{
    /// <summary>
    /// Settings for <see cref="DelayImportScheduler"/>
    /// </summary>
    internal class DelayImportSchedulerSettings
    {
        public const string SectionName = "DelayImportScheduler";
        public TimeSpan ImportDelay { get; set; }
    }
}
