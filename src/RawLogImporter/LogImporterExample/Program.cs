using LogChugger;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Extensions.Logging;
using System;

namespace LogImporterExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .WriteTo.Console()
              .CreateLogger();

            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("settings.json")
                .Build();

            config.GetRequiredSection("delayImportScheduler");

            RawLogManager logManager = new RawLogManager(
                new SerilogLoggerFactory(),
                config);

            logManager.Start();
        }
    }
}
