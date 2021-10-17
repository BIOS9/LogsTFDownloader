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

        public DelayImportScheduler(ILoggerFactory loggerFactory, DelayImportSchedulerSettings settings)
        {
            logger = loggerFactory.CreateLogger(nameof(DelayImportScheduler));
            this.settings = settings;
        }

        public async void Start()
        {
            lock (stopTokenSource)
            {
                if (stopTokenSource != null)
                    throw new InvalidOperationException("Import scheduler is already running.");
                stopTokenSource = new CancellationTokenSource();
            }
            while (!stopTokenSource.IsCancellationRequested)
            {
                await Task.Delay(settings.ImportDelay);
            }
        }

        public void Stop()
        {
            lock(stopTokenSource)
            {
                if (stopTokenSource == null)
                    throw new InvalidOperationException("Import scheduler is not running.");
                stopTokenSource?.Cancel();
            }
        }
    }
}
