// <copyright file="MySqlMetadataRepository.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Storage.MySqlMetadataRepository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dapper;
    using Dapper.Contrib.Extensions;
    using MySql.Data.MySqlClient;

    /// <summary>
    /// A MySql based metadata storage repository for raw log metadata.
    /// </summary>
    internal class MySqlMetadataRepository : IRawLogMetadataRepository
    {
        internal const string RawLogTable = "RawLogs";

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
        public Task<ICollection<RawLogMetadata>> GetMetadataByHashAsync(byte[] hash)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<RawLogMetadata> GetMetadataByIdAsync(int id)
        {
            using (var connection = new MySqlConnection(this.settings.ConnectionString))
            {
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
        }

        /// <inheritdoc/>
        public Task<ICollection<int>> GetUnassignedLogIdsAsync(int max)
        {
            throw new System.NotImplementedException();
        }
    }
}
