// <copyright file="IRemoteLogSource.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Remote
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A remote source of logs that contains logs to be imported.
    /// </summary>
    internal interface IRemoteLogSource
    {
        /// <summary>
        /// Gets latest log ID from the remote source that is not past the <paramref name="ignorePast"/> time.
        /// </summary>
        /// <param name="ignorePast">Ignores any logs created past this date.</param>
        /// <returns>Latest log ID.</returns>
        public Task<int> GetLatestLogIDAsync(DateTime ignorePast);

        /// <summary>
        /// Gets log content from the remote source.
        /// </summary>
        /// <param name="id">ID of the log to get.</param>
        /// <returns>Raw log JSON string.</returns>
        public Task<string> GetLogAsync(int id);
    }
}
