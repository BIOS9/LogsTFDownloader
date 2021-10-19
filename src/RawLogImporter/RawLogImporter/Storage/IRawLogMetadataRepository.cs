// <copyright file="IRawLogMetadataRepository.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Storage
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// A repository that stores metadata about logs such as log IDs and import status.
    /// </summary>
    public interface IRawLogMetadataRepository
    {
        /// <summary>
        /// Returns all log IDs with no status from 0 (Exclusive) to <paramref name="max"/>.
        /// </summary>
        /// <param name="max">Maximum log ID to include in the search.</param>
        /// <returns>List of all unassigned log IDs.</returns>
        public Task<ICollection<int>> GetUnassignedLogIdsAsync(int max);

        /// <summary>
        /// Gets a metadata record using a unique log ID.
        /// </summary>
        /// <param name="id">The unique ID of the log.</param>
        /// <returns>Null or a <see cref="RawLogMetadata"/> record.</returns>
        public Task<RawLogMetadata> GetMetadataByIdAsync(int id);

        /// <summary>
        /// Updates raw log metadata in the repository.
        /// </summary>
        /// <param name="metadata">Metadata to update.</param>
        /// <returns>Asynchronous task.</returns>
        public Task UpdateMetadata(RawLogMetadata metadata);

        /// <summary>
        /// Adds one or more metadata records marked as to-download.
        /// </summary>
        /// <param name="metadata">Metadata to add.</param>
        /// <returns>Asynchronous task.</returns>
        public Task AddToDownloadMetadataAsync(params ToDownloadRawLogMetadata[] metadata);

        /// <summary>
        /// Gets the latest log ID in the repository.
        /// </summary>
        /// <returns>An integer log ID.</returns>
        public Task<int?> GetLatestLogIdAsync();

        /// <summary>
        /// Gets all of the logs that are set to the specified import status.
        /// </summary>
        /// <param name="status">The import status to filter for.</param>
        /// <returns>A collection of integerLogIDs.</returns>
        public Task<ICollection<int>> GetIdsByImportStatusAsync(RawLogMetadata.RawLogImportStatus status);
    }
}
