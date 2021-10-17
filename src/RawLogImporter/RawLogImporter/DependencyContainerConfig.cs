using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using LogChugger.Remote;
using LogChugger.Remote.LogsTFApi;
using LogChugger.Storage;
using LogChugger.Import;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LogChugger
{
    /// <summary>
    /// Dependency injection setup/configuration.
    /// </summary>
    internal static class DependencyContainerConfig
    {
        /// <summary>
        /// Configure dependency injection.
        /// </summary>
        /// <returns>Dependency container.</returns>
        public static IContainer Configure(
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(configuration).SingleInstance();
            builder.RegisterInstance(loggerFactory).SingleInstance();
            
            builder.RegisterType<DelayImportScheduler>().As<IRawLogImportScheduler>();
            builder.RegisterInstance(configuration.GetRequiredSection(DelayImportSchedulerSettings.SectionName)
                .Get<DelayImportSchedulerSettings>());

            builder.RegisterType<PolicedLogsTFApiClient>().As<IRemoteLogSource>();

            builder.RegisterType<MySqlMetadataRepository>().As<IRawLogMetadataRepository>();
            builder.RegisterType<DiskLogRepository>().As<IRawLogRepository>();

            return builder.Build();
        }
    }
}
