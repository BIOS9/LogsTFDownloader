using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LogChugger.Import
{
    /**
     * <summary>Schedules log importing based on fixed delay.</summary>
     */
    internal class DelayImportScheduler : IRawLogImportScheduler
    {
        private CancellationTokenSource stopTokenSource = null;

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
