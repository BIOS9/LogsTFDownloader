// <copyright file="PolicedLogsTFApiClientSettings.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Remote.LogsTFApi
{
    using System;

    /// <summary>
    /// Rate limiter settings for <see cref="PolicedLogsTFApiClient"/>.
    /// </summary>
    internal class PolicedLogsTFApiClientSettings
    {
        /// <summary>
        /// Gets or sets the number of requests that can be made before reaching the <see cref="AverageRequestInterval"/>.
        /// </summary>
        public int BurstRequestLimit { get; set; }

        /// <summary>
        /// Gets or sets the average interval between requests.
        /// </summary>
        public TimeSpan AverageRequestInterval { get; set; }
    }
}
