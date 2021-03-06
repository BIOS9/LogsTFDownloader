// <copyright file="LogsTFApiClient.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Remote.LogsTFApi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Communicates with the logs.tf website to get logs and log metadata.
    /// </summary>
    internal class LogsTFApiClient : IRemoteLogSource
    {
        private const int LogPageSize = 500;
        private const string LatestLogsEndpoint = "https://logs.tf/api/v1/log?limit={0}&offset={1}";
        private const string SingleLogEndpoint = "https://logs.tf/api/v1/log/{0}";
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogsTFApiClient"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory to use to create a new logger.</param>
        public LogsTFApiClient(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger(nameof(LogsTFApiClient));
        }

        /// <inheritdoc/>
        public virtual async Task<int> GetLatestLogIdAsync(DateTime ignorePast)
        {
            this.logger.LogDebug("Downloading latest log ID from logs.tf. Ignoring past {ignorePast}", ignorePast);
            for (int offset = 0; ; offset += LogPageSize)
            {
                // Generated logs page URL and download the list.
                string url = string.Format(LatestLogsEndpoint, LogPageSize, offset);
                this.logger.LogTrace("Downloading: {url}", url);
                HttpWebRequest request = WebRequest.CreateHttp(url);
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                this.logger.LogTrace("Response: {code} {message}", response.StatusCode, response.StatusDescription);

                /*
                 * TODO: Actually do something with the response code.
                 */

                // Deserialize response and find the log ID.
                JsonElement result = await JsonSerializer.DeserializeAsync<JsonElement>(response.GetResponseStream());
                if (!result.GetProperty("success").GetBoolean())
                {
                    throw new WebException("Server returned Success=false.");
                }

                JsonElement logsArray = result.GetProperty("logs");
                foreach (JsonElement log in logsArray.EnumerateArray())
                {
                    long epochTime = log.GetProperty("date").GetInt64();
                    int id = log.GetProperty("id").GetInt32();
                    DateTime time = DateTimeOffset.FromUnixTimeSeconds(epochTime).DateTime;
                    this.logger.LogTrace("Log: {id}, Epoch: {epoch}, Date: {date}", id, epochTime, time);

                    // If the log time is earlier than the ignorePast time, return that log ID.
                    if (time < ignorePast.ToUniversalTime())
                    {
                        return id;
                    }
                }
            }
        }

        /// <inheritdoc/>
        public virtual async Task<string> GetLogAsync(int id)
        {
            this.logger.LogDebug("Downloading log ID {id} from logs.tf.", id);

            // Generated log URL and download the JSON.
            string url = string.Format(SingleLogEndpoint, id);
            this.logger.LogTrace("Downloading: {url}", url);
            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)await request.GetResponseAsync();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException(response.StatusDescription);
                }

                throw ex;
            }

            using StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = await reader.ReadToEndAsync();

            // Deserialize response to check that it is valid JSON
            JsonElement element = JsonSerializer.Deserialize<JsonElement>(json);
            if (!element.GetProperty("success").GetBoolean())
            {
                throw new WebException("Server returned Success=false.");
            }

            return json;
        }
    }
}
