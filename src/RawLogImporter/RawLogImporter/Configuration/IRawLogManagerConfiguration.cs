using System;

namespace RawLogManager.Configuration
{
    interface IRawLogManagerConfiguration
    {
        /**
         * <summary>Time between an import finishing and the next one starting.</summary>
         */
        TimeSpan ImportDelay { get; }

        /**
         * <summary>Number of requests that can be made before encoutering the rate limiter.</summary>
         */
        int RateLimitBurstCount { get; }

        /**
         * <summary>How often a request can be made on average.</summary>
         */
        TimeSpan RateLimitInterval { get; }

        /**
         * <summary>How long to sleep for when the server returns
         * a HTTP 429 (too many requests) or the request times out.</summary>
         */
        TimeSpan RateLimitErrorDelay { get; }
    }
}
