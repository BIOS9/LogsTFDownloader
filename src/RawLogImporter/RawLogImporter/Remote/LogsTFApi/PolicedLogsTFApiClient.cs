using Microsoft.Extensions.Logging;

namespace LogChugger.Remote.LogsTFApi
{
    /// <summary>
    /// A rate-limited version of <see cref="LogsTFApi"/>
    /// </summary>
    internal class PolicedLogsTFApiClient : LogsTFApiClient
    {
        public PolicedLogsTFApiClient(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    }
}
