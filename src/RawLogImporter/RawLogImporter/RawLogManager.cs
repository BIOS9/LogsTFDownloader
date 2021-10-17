﻿using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using LogChugger.Import;
using LogChugger.Storage;
using System;

namespace LogChugger
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
