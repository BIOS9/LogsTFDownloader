// <copyright file="DiskLogRepository.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Storage.DiskLogRepository
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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
            string logFile = Path.Combine(this.logsDirectory, id + ".json");

            // Probably don't even need this check, the delete would fail anyway but this probably gives a nicer error.
            if (!File.Exists(logFile))
            {
                throw new FileNotFoundException($"The log file at \"{logFile}\" does not exist.");
            }

            File.Delete(logFile);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<bool> DoesLogExistAsync(int id)
        {
            this.logger.LogDebug("Checking if log exists {id}", id);
            string logFile = Path.Combine(this.logsDirectory, id + ".json");
            return Task.FromResult(File.Exists(logFile));
        }

        /// <inheritdoc/>
        public Task<ICollection<int>> GetAllLogIDsAsync()
        {
            this.logger.LogDebug("Getting all log IDs (this may take a while).");

            if (!Directory.Exists(this.logsDirectory))
            {
                return Task.FromResult((ICollection<int>)new int[0]);
            }

            // This isn't implemented asynchronously at all. It will block but I don't care.
            string[] fileNames = Directory.GetFiles(this.logsDirectory);
            int[] ids = fileNames
                .Where(file => file.EndsWith(".json"))
                .Select(file => file.Remove(file.Length - 5))
                .Select(id => int.Parse(id))
                .ToArray();
            return Task.FromResult((ICollection<int>)ids);
        }

        /// <inheritdoc/>
        public async Task<string> GetLogAsync(int id)
        {
            this.logger.LogDebug("Getting log content {id}", id);
            string logFile = Path.Combine(this.logsDirectory, id + ".json");

            if (!File.Exists(logFile))
            {
                throw new FileNotFoundException($"The log file at \"{logFile}\" does not exist.");
            }

            return await File.ReadAllTextAsync(logFile);
        }

        /// <inheritdoc/>
        public async Task SaveLogAsync(string content, int id)
        {
            this.logger.LogDebug("Saving log {id}", id);
            string logFile = Path.Combine(this.logsDirectory, id + ".json");

            // Create the logs directory if it doesn't already exist.
            if (!Directory.Exists(this.logsDirectory))
            {
                Directory.CreateDirectory(this.logsDirectory);
            }

            // Check if the log already exists.
            if (File.Exists(logFile))
            {
                throw new IOException($"The log file at \"{logFile}\" already exists.");
            }

            await File.WriteAllTextAsync(logFile, content);
        }
    }
}
