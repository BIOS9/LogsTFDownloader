using LogChugger;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Extensions.Logging;
using System;

namespace LogImporterExample
{
    class Program
    {
        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .WriteTo.Console()
              .MinimumLevel.Verbose()
              .CreateLogger();

            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("settings.dev.json")
                .Build();

            RawLogManager logManager = new RawLogManager(
                new SerilogLoggerFactory(),
                config);

            logManager.Start();
            Console.ReadLine();
        }
    }
}
