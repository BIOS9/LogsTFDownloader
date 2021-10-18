// <copyright file="IRawLogImporter.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Import
{
    using System.Threading.Tasks;

    /// <summary>
    /// Imports a single log.
    /// </summary>
    internal interface IRawLogImporter
    {
        /// <summary>
        /// Imports a single log into the local storage repositories.
        /// </summary>
        /// <param name="id">Unique ID for the log.</param>
        /// <returns>Asynchronous task.</returns>
        public Task ImportLogAsync(int id);
    }
}
