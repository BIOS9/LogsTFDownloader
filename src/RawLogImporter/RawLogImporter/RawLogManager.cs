namespace LogChugger
{
    using Autofac;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using LogChugger.Import;
    using LogChugger.Storage;
    using System;

    /// <summary>
    /// Combines multiple sub-components to acheive an up-to-date and organised local
    /// repository of logs.
    /// </summary>
    public class RawLogManager
    {
        private readonly IRawLogImportScheduler rawLogImportScheduler;

        public RawLogManager(
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            if(loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            IContainer dependencyContainer = DependencyContainerConfig.Configure(loggerFactory, configuration);
            rawLogImportScheduler = dependencyContainer.Resolve<IRawLogImportScheduler>();
        }

        public void Start()
        {
            rawLogImportScheduler.Start();
        }

        public void Stop()
        {
            rawLogImportScheduler.Stop();
        }
    }
}
