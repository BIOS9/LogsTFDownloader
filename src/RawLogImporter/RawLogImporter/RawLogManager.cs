using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RawLogManager.Configuration;
using System;

namespace RawLogManager
{
    public class RawLogManager
    {
        public readonly RawLogManagerConfiguration configuration;

        public RawLogManager(IConfiguration configuration, ILoggerFactory loggerFactory)
        {

        }

        public async void Start()
        {

        }

        public async void Stop()
        {

        }
    }
}
