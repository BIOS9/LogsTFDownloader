// <copyright file="RawLogMetadata.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Storage
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a record in the raw log metadata repository.
    /// </summary>
    public class RawLogMetadata
    {
        /// <summary>
        /// Import status of a log.
        /// </summary>
        public enum RawLogImportStatus
        {
            /// <summary>
            /// Log is queued to be imported.
            /// </summary>
            ToImport,

            /// <summary>
            /// Log has been successfully imported.
            /// </summary>
            Succeed,

            /// <summary>
            /// Log import was attempted and failed.
            /// </summary>
            Failed,

            /// <summary>
            /// Log was not found on the remote log source.
            /// </summary>
            NotFound,
        }

        /// <summary>
        /// Gets or sets the unique log ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the current import status of this log.
        /// </summary>
        public RawLogImportStatus ImportStatus { get; set; }

        /// <summary>
        /// Gets or sets the failure message.
        /// Populated when a log import status is set to failed.
        /// </summary>
        public string FailureMessage { get; set; }

        /// <summary>
        /// Gets or sets the hash of the log used for duplicate detection.
        /// </summary>
        public byte[] Hash { get; set; }

        /// <summary>
        /// Gets or sets IDs for duplicate logs.
        /// </summary>
        public ICollection<int> DuplicateLogs { get; set; }

        /// <summary>
        /// Gets or sets the time that the log was added or updated.
        /// </summary>
        public DateTime Time { get; set; }
    }
}
