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
        internal const string DuplicatesTable = "logsrawduplicates";

        private readonly MySqlMetadataRepositorySettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlMetadataRepository"/> class.
        /// </summary>
        /// <param name="settings">MySql settings.</param>
        public MySqlMetadataRepository(MySqlMetadataRepositorySettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <inheritdoc/>
        public async Task AddMetadata(RawLogMetadata metadata)
        {
            using var connection = new MySqlConnection(this.settings.ConnectionString);
            using var transaction = await connection.BeginTransactionAsync();

            Models.RawLog rawLog = new Models.RawLog
            {
                Id = metadata.Id,
                ImportStatus = metadata.ImportStatus,
                FailureMessage = metadata.FailureMessage,
                Hash = metadata.Hash,
                DuplicateId = null,
                Time = metadata.Time,
            };

            if (metadata.DuplicateLogs.Any())
            {
                int duplicateId = await connection.QuerySingleAsync<int>(
                    $"INSERT INTO `{DuplicatesTable}` (`id`) VALUES (NULL);");
                rawLog.DuplicateId = duplicateId;

                foreach (int duplicateLog in metadata.DuplicateLogs)
                {
                    await connection.ExecuteAsync(
                        $"UPDATE `{RawLogTable}` SET `DuplicateId` = @duplicateId WHERE `id` = @logId",
                        new
                        {
                            logId = duplicateLog,
                            duplicateId,
                        });
                }
            }

            await connection.InsertAsync(rawLog);
            await transaction.CommitAsync();
        }

        /// <inheritdoc/>
        public async Task<ICollection<int>> GetIdsByHashAsync(byte[] hash)
        {
            using var connection = new MySqlConnection(this.settings.ConnectionString);
            IEnumerable<int> ids = await connection.QueryAsync<int>(
                $"SELECT `id` FROM `{RawLogTable}` WHERE `hash` = @hash",
                new { hash });
            return ids.AsList();
        }

        /// <inheritdoc/>
        public async Task<RawLogMetadata> GetMetadataByIdAsync(int id)
        {
            using var connection = new MySqlConnection(this.settings.ConnectionString);
            Models.RawLog log = await connection.GetAsync<Models.RawLog>(id);
            IEnumerable<int> duplicateIds = await connection.QueryAsync<int>(
                $"SELECT `id` FROM `{RawLogTable}` WHERE `duplicateId` = @duplicateId",
                new { duplicateId = log.DuplicateId });

            return new RawLogMetadata
            {
                Id = log.Id,
                ImportStatus = log.ImportStatus,
                FailureMessage = log.FailureMessage,
                Hash = log.Hash,
                DuplicateLogs = duplicateIds.AsList(),
                Time = log.Time,
            };
        }

        /// <inheritdoc/>
        public Task<ICollection<int>> GetUnassignedLogIdsAsync(int max)
        {
            throw new System.NotImplementedException();
        }
    }
}
