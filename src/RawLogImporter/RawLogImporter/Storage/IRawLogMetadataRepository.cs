// <copyright file="IRawLogMetadataRepository.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Storage
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// A repository that stores metadata about logs such as log IDs, hashes, import status
    /// and information about duplicate logs.
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
        /// Gets the IDs for all logs with the same has .
        /// </summary>
        /// <param name="hash">The hash to search for.</param>
        /// <returns>A collection with 0 or more log IDs.</returns>
        public Task<ICollection<int>> GetIdsByHashAsync(byte[] hash);

        /// <summary>
        /// Adds raw log metadata to the repository.
        /// </summary>
        /// <param name="metadata">Metadata to add.</param>
        /// <returns>Asynchronous task.</returns>
        public Task AddMetadata(RawLogMetadata metadata);
    }
}
