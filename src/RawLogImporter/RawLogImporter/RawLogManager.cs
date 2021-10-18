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

        /// <summary>
        /// Initializes a new instance of the <see cref="RawLogManager"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory used to create a new logger for each component.</param>
        /// <param name="configuration">The configuration that contains all the requred sections for each sub-component.</param>
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

        /// <summary>
        /// Start management of logs and associated tasks such as import and clean up.
        /// </summary>
        public void Start()
        {
            this.rawLogImportScheduler.Start();
        }

        /// <summary>
        /// Stop management of logs and all associated taks.
        /// </summary>
        public void Stop()
        {
            this.rawLogImportScheduler.Stop();
        }
    }
}
