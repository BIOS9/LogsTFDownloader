// <copyright file="DiskLogRepository.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Storage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A local disk filesystem based storage repository for raw JSON logs.
    /// </summary>
    internal class DiskLogRepository : IRawLogRepository
    {
        private readonly ILogger logger;
        private readonly DiskLogRepositorySettings settings;
        private readonly string logsDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiskLogRepository"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory used to create logger.</param>
        /// <param name="settings">Repository settings.</param>
        public DiskLogRepository(ILoggerFactory loggerFactory, DiskLogRepositorySettings settings)
        {
            this.logger = loggerFactory.CreateLogger(nameof(DiskLogRepository));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.logsDirectory = Path.GetFullPath(settings.LogsDirectoryPath);
            this.logger.LogInformation("Using path {path} for log storage", this.logsDirectory);
        }

        /// <inheritdoc/>
        public Task DeleteLogAsync(int id)
        {
            this.logger.LogDebug("Deleting log {id}", id);
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<bool> DoesLogExistAsync(int id)
        {
            this.logger.LogDebug("Checking if log exists {id}", id);
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<ICollection<int>> GetAllLogIDsAsync()
        {
            this.logger.LogDebug("Getting all log IDs (this may take a while).");
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<string> GetLogAsync(int id)
        {
            this.logger.LogDebug("Getting log content {id}", id);
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task SaveLogAsync(string content, int id)
        {
            this.logger.LogDebug("Saving log {id}", id);
            throw new NotImplementedException();
        }
    }
}
