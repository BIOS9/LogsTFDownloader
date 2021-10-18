// <copyright file="DelayImportScheduler.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Import.DelayImportScheduler
{
    using System;
    using System.IO;
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
                    int latestLogID = await this.remoteLogSource.GetLatestLogIDAsync(DateTime.Now - TimeSpan.FromHours(1));
                    this.logger.LogInformation(latestLogID.ToString());
                    await this.logImporter.ImportLogAsync(latestLogID);
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
