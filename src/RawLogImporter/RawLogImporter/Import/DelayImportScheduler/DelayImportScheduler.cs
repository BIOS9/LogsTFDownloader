// <copyright file="DelayImportScheduler.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Import.DelayImportScheduler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using LogChugger.Remote;
    using LogChugger.Storage;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Schedules log importing based on fixed delay.
    /// </summary>
    internal class DelayImportScheduler : IRawLogImportScheduler
    {
        private readonly ILogger logger;
        private readonly DelayImportSchedulerSettings settings;
        private readonly IRemoteLogSource remoteLogSource;
        private readonly IRawLogMetadataRepository metadataRepository;
        private readonly IRawLogImporter logImporter;
        private readonly object startStopLock = new object();
        private CancellationTokenSource stopTokenSource = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelayImportScheduler"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory used to create a logger.</param>
        /// <param name="settings">Import scheduler settings.</param>
        /// <param name="remoteLogSource">The remote log source to use to fetch logs.</param>
        /// <param name="metadataRepository">The metadata repository that stores the raw log information.</param>
        /// <param name="logImporter">Raw log importer used to import logs into the local repositories periodically.</param>
        public DelayImportScheduler(
            ILoggerFactory loggerFactory,
            DelayImportSchedulerSettings settings,
            IRemoteLogSource remoteLogSource,
            IRawLogMetadataRepository metadataRepository,
            IRawLogImporter logImporter)
        {
            this.logger = loggerFactory.CreateLogger(nameof(DelayImportScheduler));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.remoteLogSource = remoteLogSource ?? throw new ArgumentNullException(nameof(remoteLogSource));
            this.metadataRepository = metadataRepository ?? throw new ArgumentNullException(nameof(metadataRepository));
            this.logImporter = logImporter ?? throw new ArgumentNullException(nameof(logImporter));
        }

        /// <inheritdoc/>
        public async void Start()
        {
            lock (this.startStopLock)
            {
                if (this.stopTokenSource != null && !this.stopTokenSource.IsCancellationRequested)
                {
                    throw new InvalidOperationException("Import scheduler is already running.");
                }

                this.stopTokenSource = new CancellationTokenSource();
            }

            while (!this.stopTokenSource.IsCancellationRequested)
            {
                try
                {
                    // Get latest log ID that is at least 1 hour old. This is to prevent grabbing logs for running games.
                    int latestRemoteLogId = await this.remoteLogSource.GetLatestLogIdAsync(DateTime.Now - TimeSpan.FromHours(1));
                    int latestLocalLogId = await this.metadataRepository.GetLatestLogIdAsync() ?? 0;
                    this.logger.LogDebug("Latest remote ID: {remote}, latest local id: {local}", latestRemoteLogId, latestLocalLogId);

                    this.logger.LogDebug("Queueing missing logs for import.");
                    for (int i = latestLocalLogId + 1; i <= latestRemoteLogId; ++i)
                    {
                        await this.metadataRepository.AddToDownloadMetadataAsync(new ToDownloadRawLogMetadata
                        {
                            Id = i,
                            Time = DateTime.Now,
                        });
                    }

                    ICollection<int> toImportLogs = await this.metadataRepository.GetIdsByImportStatusAsync(RawLogMetadata.RawLogImportStatus.ToImport);
                    ICollection<int> failedLogs = await this.metadataRepository.GetIdsByImportStatusAsync(RawLogMetadata.RawLogImportStatus.Failed);
                    foreach (int id in failedLogs.Concat(toImportLogs))
                    {
                        await this.logImporter.ImportLogAsync(id);
                    }

                    await Task.Delay(this.settings.ImportDelay);
                }
                catch (IOException ex)
                {
                    this.logger.LogError(ex, "Failed to download resource from remote.");
                }
            }
        }

        /// <inheritdoc/>
        public void Stop()
        {
            lock (this.startStopLock)
            {
                if (this.stopTokenSource == null || this.stopTokenSource.IsCancellationRequested)
                {
                    throw new InvalidOperationException("Import scheduler is not running.");
                }

                this.stopTokenSource?.Cancel();
            }
        }
    }
}
