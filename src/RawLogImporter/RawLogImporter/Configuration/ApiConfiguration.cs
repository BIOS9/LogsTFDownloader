using System;
using System.Collections.Generic;
using System.Text;

namespace RawLogManager.Configuration
{
    public class ApiConfiguration
    {
        public readonly int rateLimitBurstCount;
        public readonly TimeSpan rateLimitInterval;
        public readonly TimeSpan rateLimitErrorDelay;

        /**
         * <param name="rateLimitBurstCount">Number of requests that can be made before
         * encoutering the rate limiter.</param>
         * <param name="rateLimitErrorDelay">How often a request can be made on average.</param>
         * <param name="rateLimitInterval">How long to sleep for when the server returns
         * a HTTP 429 (too many requests) or the request times out.</param>
         */
        public ApiConfiguration(int rateLimitBurstCount, TimeSpan rateLimitInterval, TimeSpan rateLimitErrorDelay)
        {
            this.rateLimitBurstCount = rateLimitBurstCount;
            this.rateLimitInterval = rateLimitInterval;
            this.rateLimitErrorDelay = rateLimitErrorDelay;
        }
    }
}
