using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RawLogManager.Configuration;
using System;

namespace RawLogManager
{
    public class RawLogManager
    {
        public RawLogManager(
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            if(loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            IContainer dependencyContainer = DependencyContainerConfig.Configure(loggerFactory, configuration);
            using (ILifetimeScope dependencyScope = dependencyContainer.BeginLifetimeScope())
            {

            }
        }

        public async void Start()
        {

        }

        public async void Stop()
        {

        }
    }
}
