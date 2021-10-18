// <copyright file="DualLogImporter.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Import.DualLogImporter
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using LogChugger.Remote;
    using LogChugger.Storage;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Log importer that sources logs from both the local raw log repository
    /// and the remote log source.
    /// This allows bulk importing of existing logs without having to use
    /// the remote log source.
    /// </summary>
    internal class DualLogImporter : IRawLogImporter
    {
        private readonly ILogger logger;
        private readonly IRemoteLogSource remoteLogSource;
        private readonly IRawLogMetadataRepository metadataRepository;
        private readonly IRawLogRepository logRepository;
        private readonly DualLogImporterSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DualLogImporter"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory used to create new logger instance.</param>
        /// <param name="remoteLogSource">Remote log source used to get missing logs.</param>
        /// <param name="metadataRepository">Metadata repo to save imported log metadata to.</param>
        /// <param name="logRepository">Log repo to save raw imported logs to.</param>
        /// <param name="settings">Settings for <see cref="DualLogImporter"/>.</param>
        public DualLogImporter(
            ILoggerFactory loggerFactory,
            IRemoteLogSource remoteLogSource,
            IRawLogMetadataRepository metadataRepository,
            IRawLogRepository logRepository,
            DualLogImporterSettings settings)
        {
            this.logger = loggerFactory.CreateLogger(nameof(DualLogImporter));
            this.remoteLogSource = remoteLogSource ?? throw new ArgumentNullException(nameof(remoteLogSource));
            this.metadataRepository = metadataRepository ?? throw new ArgumentNullException(nameof(metadataRepository));
            this.logRepository = logRepository ?? throw new ArgumentNullException(nameof(logRepository));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <inheritdoc/>
        public async Task ImportLogAsync(int id)
        {
            this.logger.LogDebug("Importing log ID: {ID}", id);
            string logContent = await this.FindLogContentAsync(id);
            DuplicateSearchResult duplicateSearchResult = await this.FindDuplicateLogsAsync(id, logContent);

            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Finds the log ID content from either the local repository (preferred)
        /// or the remote log source.
        /// </summary>
        /// <param name="id">Unique log ID.</param>
        /// <returns>Log content string.</returns>
        private async Task<string> FindLogContentAsync(int id)
        {
            string logContent;
            bool isLocal; // Whether the log is in the local repo or not.

            // Get log content.
            if (await this.logRepository.DoesLogExistAsync(id))
            {
                isLocal = true;
                this.logger.LogTrace("Log {id} found in local repository.", id);
                logContent = await this.logRepository.GetLogAsync(id);
            }
            else
            {
                isLocal = false;
                this.logger.LogTrace("Log {id} not found in local repository. Importing from remote...", id);
                logContent = await this.remoteLogSource.GetLogAsync(id);
            }

            // Validate JSON.
            try
            {
                _ = JsonDocument.Parse(logContent);
            }
            catch (JsonException ex)
            {
                // If log is local delete it since it's invalid.
                if (isLocal)
                {
                    await this.logRepository.DeleteLogAsync(id);
                }

                throw ex;
            }

            return logContent;
        }

        private struct DuplicateSearchResult
        {
            public ICollection<int> DuplicateLogs;
            public byte[] LogHash;
        }

        /// <summary>
        /// Finds duplicate logs in the local repository and also calculates the hash
        /// to be saved when the log is imported.
        /// </summary>
        /// <param name="content">JSON content of the log.</param>
        /// <returns>
        /// A <see cref="DuplicateSearchResult"/> which contains the IDs of duplicate logs
        /// and the log hash.
        /// </returns>
        private async Task<DuplicateSearchResult> FindDuplicateLogsAsync(int id, string content)
        {
            byte[] contentBytes = Encoding.Unicode.GetBytes(content);
            byte[] hashBytes;
            using (HashAlgorithm hash = HashAlgorithm.Create(this.settings.DuplicateCheckHashAlgorithm))
            {
                hashBytes = hash.ComputeHash(contentBytes);
            }
            this.logger.LogTrace(
                "Finding duplicates for log {id} {hashAlgo} hash {hash}.",
                id,
                this.settings.DuplicateCheckHashAlgorithm,
                Convert.ToBase64String(hashBytes));

            return new DuplicateSearchResult();
        }
    }
}
