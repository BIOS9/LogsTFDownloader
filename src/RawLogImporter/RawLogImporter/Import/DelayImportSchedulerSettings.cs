// <copyright file="DelayImportSchedulerSettings.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Import
{
    using System;

    /// <summary>
    /// Settings for <see cref="DelayImportScheduler"/>.
    /// </summary>
    internal class DelayImportSchedulerSettings
    {
        public const string SectionName = "DelayImportScheduler";

        public TimeSpan ImportDelay { get; set; }
    }
}
