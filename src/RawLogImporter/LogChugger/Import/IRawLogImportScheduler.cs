// <copyright file="IRawLogImportScheduler.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Import
{
    /// <summary>
    /// Handles scheduling and triggering of batch log imports.
    /// </summary>
    public interface IRawLogImportScheduler
    {
        /// <summary>
        /// Starts collecting logs periodically.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops periodic log collection.
        /// </summary>
        void Stop();
    }
}
