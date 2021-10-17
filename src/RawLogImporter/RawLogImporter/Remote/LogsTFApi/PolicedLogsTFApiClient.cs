using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogChugger.Remote.LogsTFApi
{
    internal class PolicedLogsTFApiClient : LogsTFApiClient
    {
        public PolicedLogsTFApiClient(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    }
}
