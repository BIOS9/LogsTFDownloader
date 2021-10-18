// <copyright file="DelayImportSchedulerSettings.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Import.DelayImportScheduler
{
    using System;

    /// <summary>
    /// Settings for <see cref="DelayImportScheduler"/>.
    /// </summary>
    internal class DelayImportSchedulerSettings
    {
        /// <summary>
        /// The section name to be used in the config file.
        /// </summary>
        public const string SectionName = "DelayImportScheduler";

        /// <summary>
        /// Gets or sets the amount of time to wait between imports.
        /// </summary>
        public TimeSpan ImportDelay { get; set; }
    }
}
