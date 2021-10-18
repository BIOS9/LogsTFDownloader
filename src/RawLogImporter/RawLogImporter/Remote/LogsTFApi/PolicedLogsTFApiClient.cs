// <copyright file="PolicedLogsTFApiClient.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Remote.LogsTFApi
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A rate-limited version of <see cref="LogsTFApi"/>.
    /// </summary>
    internal class PolicedLogsTFApiClient : LogsTFApiClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolicedLogsTFApiClient"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory to be used to create a new logger.</param>
        public PolicedLogsTFApiClient(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }
    }
}
