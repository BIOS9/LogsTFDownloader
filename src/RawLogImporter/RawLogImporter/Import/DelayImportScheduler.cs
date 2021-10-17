using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
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

        public DelayImportScheduler(ILoggerFactory loggerFactory, DelayImportSchedulerSettings settings)
        {
            logger = loggerFactory.CreateLogger(nameof(DelayImportScheduler));
            this.settings = settings;
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
                logger.LogInformation(settings.ImportDelay.ToString());
                await Task.Delay(settings.ImportDelay);
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
