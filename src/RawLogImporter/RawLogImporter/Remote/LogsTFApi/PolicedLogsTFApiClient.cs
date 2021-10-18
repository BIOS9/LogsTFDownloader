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
        public PolicedLogsTFApiClient(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }
    }
}
