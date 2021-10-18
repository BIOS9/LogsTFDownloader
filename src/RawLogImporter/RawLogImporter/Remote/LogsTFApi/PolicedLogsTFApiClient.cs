// <copyright file="PolicedLogsTFApiClient.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Remote.LogsTFApi
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A rate-limited version of <see cref="LogsTFApi"/>.
    /// Rate limiting is based on the token bucket algorithm https://en.wikipedia.org/wiki/Token_bucket.
    /// </summary>
    internal class PolicedLogsTFApiClient : LogsTFApiClient, IDisposable
    {
        private readonly ILogger logger;
        private readonly PolicedLogsTFApiClientSettings settings;
        private readonly SemaphoreSlim requestTokenBucket;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicedLogsTFApiClient"/> class.
        /// </summary>
        /// <param name="settings">Rate limiter settings for this policer.</param>
        /// <param name="loggerFactory">The logger factory to be used to create a new logger.</param>
        public PolicedLogsTFApiClient(PolicedLogsTFApiClientSettings settings, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.logger = loggerFactory.CreateLogger(nameof(PolicedLogsTFApiClient));
            this.requestTokenBucket = new SemaphoreSlim(settings.BurstRequestLimit, settings.BurstRequestLimit);
            this.StartTokenLoopAsync();
        }

        /// <summary>
        /// Gets latest log ID from the remote source that is not past the <paramref name="ignorePast"/> time
        /// while adhering to rate limits specified in <see cref="PolicedLogsTFApiClientSettings"/>.
        /// </summary>
        /// <param name="ignorePast">Ignores any logs created past this date.</param>
        /// <returns>Latest log ID.</returns>
        public override async Task<int> GetLatestLogIDAsync(DateTime ignorePast)
        {
            await this.requestTokenBucket.WaitAsync();
            this.logger.LogTrace("Used one request token. Current count: {count}", this.requestTokenBucket.CurrentCount);
            return await base.GetLatestLogIDAsync(ignorePast);
        }

        /// <summary>
        /// Gets log content from the remote source while adhering to
        /// rate limits specified in <see cref="PolicedLogsTFApiClientSettings"/>.
        /// </summary>
        /// <param name="id">ID of the log to get.</param>
        /// <returns>Raw log JSON string.</returns>
        public override async Task<string> GetLogAsync(int id)
        {
            await this.requestTokenBucket.WaitAsync();
            this.logger.LogTrace("Used one request token. Current count: {count}", this.requestTokenBucket.CurrentCount);
            return await base.GetLogAsync(id);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Starts a loop that releases one request token per cycle.
        /// Cycle duration is specified in <see cref="PolicedLogsTFApiClientSettings"/>.
        /// </summary>
        private async void StartTokenLoopAsync()
        {
            while (!this.cancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(this.settings.AverageRequestInterval, this.cancellationTokenSource.Token);
                if (this.requestTokenBucket.CurrentCount < this.settings.BurstRequestLimit)
                {
                    this.logger.LogTrace("Dispensing one request token. Current count: {count}", this.requestTokenBucket.CurrentCount + 1);
                    this.requestTokenBucket.Release();
                }
            }
        }
    }
}
