// <copyright file="MySqlMetadataRepository.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Storage.MySqlMetadataRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper;
    using Dapper.Contrib.Extensions;
    using Microsoft.Extensions.Logging;
    using MySql.Data.MySqlClient;

    /// <summary>
    /// A MySql based metadata storage repository for raw log metadata.
    /// </summary>
    internal class MySqlMetadataRepository : IRawLogMetadataRepository
    {
        /// <summary>
        /// The MySql table for raw log metadata.
        /// </summary>
        internal const string RawLogTable = "logsraw";

        private readonly MySqlMetadataRepositorySettings settings;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlMetadataRepository"/> class.
        /// </summary>
        /// <param name="settings">MySql settings.</param>
        /// /// <param name="loggerFactory">Used to create a local logger instance.</param>
        public MySqlMetadataRepository(MySqlMetadataRepositorySettings settings, ILoggerFactory loggerFactory)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.logger = loggerFactory.CreateLogger(nameof(MySqlMetadataRepository));
        }

        /// <inheritdoc/>
        public async Task UpdateMetadata(RawLogMetadata metadata)
        {
            using var connection = new MySqlConnection(this.settings.ConnectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            Models.RawLog rawLog = new Models.RawLog
            {
                Id = metadata.Id,
                ImportStatus = metadata.ImportStatus.ToString(),
                FailureMessage = metadata.FailureMessage,
                Time = new DateTimeOffset(metadata.Time.ToUniversalTime()).ToUnixTimeSeconds(),
            };

            if (!await connection.UpdateAsync(rawLog))
            {
                throw new KeyNotFoundException("Log metadata was not updated.");
            }

            await transaction.CommitAsync();
        }

        /// <inheritdoc/>
        public async Task AddToDownloadMetadataAsync(ToDownloadRawLogMetadata metadata)
        {
            this.logger.LogDebug("Adding to-download log {id}", metadata.Id);
            using var connection = new MySqlConnection(this.settings.ConnectionString);
            await connection.OpenAsync();

            Models.RawLog rawLog = new Models.RawLog
            {
                Id = metadata.Id,
                ImportStatus = RawLogMetadata.RawLogImportStatus.ToImport.ToString(),
                FailureMessage = null,
                Time = new DateTimeOffset(metadata.Time.ToUniversalTime()).ToUnixTimeSeconds(),
            };

            await connection.InsertAsync(rawLog);
        }

        /// <inheritdoc/>
        public async Task<RawLogMetadata> GetMetadataByIdAsync(int id)
        {
            using var connection = new MySqlConnection(this.settings.ConnectionString);
            Models.RawLog log = await connection.GetAsync<Models.RawLog>(id);

            return new RawLogMetadata
            {
                Id = log.Id,
                ImportStatus = (RawLogMetadata.RawLogImportStatus)Enum.Parse(typeof(RawLogMetadata.RawLogImportStatus), log.ImportStatus),
                FailureMessage = log.FailureMessage,
                Time = DateTimeOffset.FromUnixTimeSeconds(log.Time).DateTime,
            };
        }

        /// <inheritdoc/>
        public Task<ICollection<int>> GetUnassignedLogIdsAsync(int max)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<int?> GetLatestLogIdAsync()
        {
            using var connection = new MySqlConnection(this.settings.ConnectionString);
            int count = await connection.QuerySingleAsync<int>(
                $"SELECT COUNT(`id`) FROM `{RawLogTable}`;");
            if (count == 0)
            {
                return null;
            }

            return await connection.QuerySingleAsync<int>(
                $"SELECT `id` FROM `{RawLogTable}` ORDER BY `id` DESC LIMIT 1;");
        }

        /// <inheritdoc/>
        public async Task<ICollection<int>> GetIdsByImportStatusAsync(RawLogMetadata.RawLogImportStatus status)
        {
            using var connection = new MySqlConnection(this.settings.ConnectionString);
            IEnumerable<int> ids = await connection.QueryAsync<int>(
                $"SELECT `id` FROM `{RawLogTable}` WHERE `ImportStatus` = @importStatus",
                new { importStatus = status.ToString() });

            return ids.ToList();
        }
    }
}
