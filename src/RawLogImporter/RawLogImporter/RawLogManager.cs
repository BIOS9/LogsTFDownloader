// <copyright file="RawLogManager.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger
{
    using System;
    using Autofac;
    using LogChugger.Import;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

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
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            IContainer dependencyContainer = DependencyContainerConfig.Configure(loggerFactory, configuration);
            this.rawLogImportScheduler = dependencyContainer.Resolve<IRawLogImportScheduler>();
        }

        public void Start()
        {
            this.rawLogImportScheduler.Start();
        }

        public void Stop()
        {
            this.rawLogImportScheduler.Stop();
        }
    }
}
