// <copyright file="RawLog.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Storage.MySqlMetadataRepository.Models
{
    using System;
    using Dapper.Contrib.Extensions;
    using static LogChugger.Storage.RawLogMetadata;

    /// <summary>
    /// A model to represent a single row in the MySql database.
    /// </summary>
    [Table(MySqlMetadataRepository.RawLogTable)]
    internal class RawLog
    {
        /// <summary>
        /// Gets or sets the unique log ID.
        /// </summary>
        [Key]
        public int Id { get; set; }

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
        public int? DuplicateId { get; set; }

        /// <summary>
        /// Gets or sets the time that the log was added or updated.
        /// </summary>
        public DateTime Time { get; set; }
    }
}
