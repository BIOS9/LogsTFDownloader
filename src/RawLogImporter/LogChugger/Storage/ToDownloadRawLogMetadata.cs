// <copyright file="ToDownloadRawLogMetadata.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Storage
{
    using System;

    /// <summary>
    /// Represents a to-download record in the raw log metadata repository.
    /// </summary>
    public class ToDownloadRawLogMetadata
    {
        /// <summary>
        /// Gets or sets the unique log ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the time that the log was added or updated.
        /// </summary>
        public DateTime Time { get; set; }
    }
}
