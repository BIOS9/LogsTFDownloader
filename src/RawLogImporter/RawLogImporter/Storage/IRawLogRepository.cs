// <copyright file="IRawLogRepository.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Storage
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// A storage repository for raw JSON logs.
    /// </summary>
    public interface IRawLogRepository
    {
        /// <summary>
        /// Returns a boolean indicating whether the specified log ID exists in the repository.
        /// </summary>
        /// <param name="id">Log ID to search for.</param>
        /// <returns>True if the specified log exists and false if it doesn't.</returns>
        public Task<bool> DoesLogExistAsync(int id);

        /// <summary>
        /// Returns the content of the specified log.
        /// </summary>
        /// <param name="id">ID of the log to get.</param>
        /// <returns>Text content of the log.</returns>
        public Task<string> GetLogAsync(int id);

        /// <summary>
        /// Saves a log to the repository.
        /// </summary>
        /// <param name="content">Text content of the log.</param>
        /// <param name="id">Unique ID of the log to save.</param>
        /// <returns>Asynchronous task.</returns>
        public Task SaveLogAsync(string content, int id);

        /// <summary>
        /// Deletes a log from the repository.
        /// </summary>
        /// <param name="id">Unique ID of the log to delete.</param>
        /// <returns>Asynchronous task.</returns>
        public Task DeleteLogAsync(int id);

        /// <summary>
        /// Returns the IDs of all logs present in the repository,
        /// </summary>
        /// <returns>A collection of log ID integers.</returns>
        public Task<ICollection<int>> GetAllLogIDsAsync();
    }
}
