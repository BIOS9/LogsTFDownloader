using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LogChugger.Remote.LogsTFApi
{
    internal class LogsTFApiClient : IRemoteLogSource
    {
        private const int LogPageSize = 500;
        private const string LatestLogEndpoint = "https://logs.tf/api/v1/log?limit={0}&offset={1}";
        private readonly ILogger logger;

        public LogsTFApiClient(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger(nameof(LogsTFApiClient));
        }

        public async Task<int> GetLatestLogIDAsync(DateTime ignorePast)
        {
            logger.LogDebug("Downloading latest log ID from logs.tf. Ignoring past {ignorePast}", ignorePast);
            for (int offset = 0; ; offset += LogPageSize)
            {
                string url = string.Format(LatestLogEndpoint, LogPageSize, offset);
                logger.LogTrace("Downloading: {url}", url);
                HttpWebRequest request = WebRequest.CreateHttp(url);
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                logger.LogTrace("Response: {code} {message}", response.StatusCode, response.StatusDescription);
                JsonElement result = await JsonSerializer.DeserializeAsync<JsonElement>(response.GetResponseStream());
                JsonElement logsArray = result.GetProperty("logs");
                foreach (JsonElement log in logsArray.EnumerateArray())
                {
                    // If the log time is earlier than the ignorePast time, return that log ID.
                    long epochTime = log.GetProperty("date").GetInt64();
                    int id = log.GetProperty("id").GetInt32();
                    DateTime time = DateTimeOffset.FromUnixTimeSeconds(epochTime).DateTime;
                    logger.LogTrace("Log: {id}, Epoch: {epoch}, Date: {date}", id, epochTime, time);

                    if (time < ignorePast.ToUniversalTime())
                        return id;
                }
            }
        }
    }
}
