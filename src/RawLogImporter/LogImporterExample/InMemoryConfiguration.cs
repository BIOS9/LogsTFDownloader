using System;

namespace LogChugger.Configuration
{
    public class InMemoryConfiguration
    {
        public TimeSpan ImportDelay { get; private set; }
        public int RateLimitBurstCount { get; private set; }
        public TimeSpan RateLimitInterval { get; private set; }
        public TimeSpan RateLimitErrorDelay { get; private set; }

        /**
         * <param name="importDelay">Time between an import finishing and the next one starting.</param>
         * <param name="rateLimitBurstCount">Number of requests that can be made before
         * encoutering the rate limiter.</param>
         * <param name="rateLimitErrorDelay">How often a request can be made on average.</param>
         * <param name="rateLimitInterval">How long to sleep for when the server returns
         * a HTTP 429 (too many requests) or the request times out.</param>
         */
        public InMemoryConfiguration(
            TimeSpan importDelay,
            int rateLimitBurstCount,
            TimeSpan rateLimitInterval,
            TimeSpan rateLimitErrorDelay)
        {
            ImportDelay = importDelay;
            RateLimitBurstCount = rateLimitBurstCount;
            RateLimitInterval = rateLimitInterval;
            RateLimitErrorDelay = rateLimitErrorDelay;
        }
    }
}
