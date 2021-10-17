using LogChugger.Remote;
using LogChugger.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LogChugger.Import
{
    /**
     * <summary>Schedules log importing based on fixed delay.</summary>
     */
    internal class DelayImportScheduler : IRawLogImportScheduler
    {
        private ILogger logger;
        private DelayImportSchedulerSettings settings;
        private CancellationTokenSource stopTokenSource = null;
        private object startStopLock = new object();
        private IRemoteLogSource remoteLogSource;
        private IRawLogMetadataRepository metadataRepository;

        public DelayImportScheduler(
            ILoggerFactory loggerFactory, 
            DelayImportSchedulerSettings settings,
            IRemoteLogSource remoteLogSource,
            IRawLogMetadataRepository metadataRepository)
        {
            logger = loggerFactory.CreateLogger(nameof(DelayImportScheduler));
            this.settings = settings;
            this.remoteLogSource = remoteLogSource;
            this.metadataRepository = metadataRepository;
        }

        public async void Start()
        {
            lock (startStopLock)
            {
                if (stopTokenSource != null && !stopTokenSource.IsCancellationRequested)
                    throw new InvalidOperationException("Import scheduler is already running.");
                stopTokenSource = new CancellationTokenSource();
            }
            while (!stopTokenSource.IsCancellationRequested)
            {
                try
                {
                    int latestLogID = await remoteLogSource.GetLatestLogIDAsync(DateTime.Now - TimeSpan.FromHours(1));
                    logger.LogInformation(latestLogID.ToString());
                    await Task.Delay(settings.ImportDelay);
                }
                catch(IOException ex)
                {
                    logger.LogError(ex, "Failed to download resource from remote.");
                }
            }
        }

        public void Stop()
        {
            lock(startStopLock)
            {
                if (stopTokenSource == null || stopTokenSource.IsCancellationRequested)
                    throw new InvalidOperationException("Import scheduler is not running.");
                stopTokenSource?.Cancel();
            }
        }
    }
}
