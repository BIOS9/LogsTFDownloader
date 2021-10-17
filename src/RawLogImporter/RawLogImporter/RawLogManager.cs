using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RawLogManager.Configuration;
using RawLogManager.Import;
using RawLogManager.Storage;
using System;

namespace RawLogManager
{
    public class RawLogManager
    {
        public readonly IRawLogImportScheduler rawLogImportScheduler;
        public readonly IRawLogMetadataRepository rawLogMetadataRepository;
        public readonly IRawLogRepository rawLogRepository;

        public RawLogManager(
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            if(loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            IContainer dependencyContainer = DependencyContainerConfig.Configure(loggerFactory, configuration);
            rawLogImportScheduler = dependencyContainer.Resolve<IRawLogImportScheduler>();
            rawLogMetadataRepository = dependencyContainer.Resolve<IRawLogMetadataRepository>();
            rawLogRepository = dependencyContainer.Resolve<IRawLogRepository>();
        }
    }
}
