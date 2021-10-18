// <copyright file="DualLogImporter.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Import.DualLogImporter
{
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DualLogImporter"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory used to create new logger instance.</param>
        /// <param name="remoteLogSource">Remote log source used to get missing logs.</param>
        /// <param name="metadataRepository">Metadata repo to save imported log metadata to.</param>
        /// <param name="logRepository">Log repo to save raw imported logs to.</param>
        public DualLogImporter(
            ILoggerFactory loggerFactory,
            IRemoteLogSource remoteLogSource,
            IRawLogMetadataRepository metadataRepository,
            IRawLogRepository logRepository)
        {
            this.logger = loggerFactory.CreateLogger(nameof(DualLogImporter));
            this.remoteLogSource = remoteLogSource;
            this.metadataRepository = metadataRepository;
            this.logRepository = logRepository;
        }

        /// <inheritdoc/>
        public Task ImportLogAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
