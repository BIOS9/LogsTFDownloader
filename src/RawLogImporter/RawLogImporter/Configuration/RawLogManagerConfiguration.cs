using Microsoft.Extensions.Configuration;
using System;

namespace RawLogManager.Configuration
{
    public class RawLogManagerConfiguration
    {
        public readonly TimeSpan importDelay;
        public readonly int rateLimitBurstCount;
        public readonly TimeSpan rateLimitInterval;
        public readonly TimeSpan rateLimitErrorDelay;

        /**
         * <param name="importDelay">Time between an import finishing and the next one starting.</param>
         * <param name="rateLimitBurstCount">Number of requests that can be made before
         * encoutering the rate limiter.</param>
         * <param name="rateLimitErrorDelay">How often a request can be made on average.</param>
         * <param name="rateLimitInterval">How long to sleep for when the server returns
         * a HTTP 429 (too many requests) or the request times out.</param>
         */
        public RawLogManagerConfiguration(
            TimeSpan importDelay,
            int rateLimitBurstCount,
            TimeSpan rateLimitInterval,
            TimeSpan rateLimitErrorDelay)
        {
            this.importDelay = importDelay;
            this.rateLimitBurstCount = rateLimitBurstCount;
            this.rateLimitInterval = rateLimitInterval;
            this.rateLimitErrorDelay = rateLimitErrorDelay;
        }

        public static RawLogManagerConfiguration Build(IConfiguration configuration)
        {
            // TODO: Build configuration from IConfiguration object
            return null;
        }
    }
}
