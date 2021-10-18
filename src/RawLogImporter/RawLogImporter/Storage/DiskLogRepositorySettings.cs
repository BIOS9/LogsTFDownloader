// <copyright file="DiskLogRepositorySettings.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Storage
{
    /// <summary>
    /// Settings for <see cref="DiskLogRepository"/>.
    /// </summary>
    internal class DiskLogRepositorySettings
    {
        /// <summary>
        /// The section name to be used in the config file.
        /// </summary>
        public const string SectionName = "DiskLogRepository";

        /// <summary>
        /// Gets or sets the path for the directory to store the raw logs.
        /// </summary>
        public string LogsDirectoryPath { get; set; }
    }
}
