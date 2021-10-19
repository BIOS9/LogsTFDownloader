using LogChugger;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace LogImporterExample
{
    class Program
    {
        
        public static async Task<int> Main(params string[] args)
        {
            RootCommand rootCommand = new RootCommand(
                description: "Converts an image file from one format to another.");

            Option settingsOption = new Option(
              aliases: new string[] { "--settings-file", "-s" },
              description: "The path to the JSON settings file.",
              argumentType: typeof(FileInfo),
              getDefaultValue: () => "settings.json");
            rootCommand.AddOption(settingsOption);

            rootCommand.Handler = CommandHandler.Create<FileInfo>(RunLogChugger);
            return await rootCommand.InvokeAsync(args);
        }

        /// <summary>
        /// Starts chugging logs.
        /// </summary>
        /// <param name="settingsFile">The path to the JSON settings file.</param>
        /// <param name="logLevel">The logging level. Levels: Verbose, Debug, Information, Warning, Error, Fatal.</param>
        private static async Task RunLogChugger(FileInfo settingsFile)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile(settingsFile.FullName)
                .Build();

            LogEventLevel logLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), config.GetSection("Logging").GetSection("LogLevel").Value);

            Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .WriteTo.Console()
              .MinimumLevel.Is(logLevel)
              .CreateLogger();

            RawLogManager logManager = new RawLogManager(
                new SerilogLoggerFactory(),
                config);

            logManager.Start();
            await Task.Delay(-1);
        }
    }
}
