// <copyright file="DependencyContainerConfig.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger
{
    using Autofac;
    using LogChugger.Import;
    using LogChugger.Import.DelayImportScheduler;
    using LogChugger.Import.DualLogImporter;
    using LogChugger.Remote;
    using LogChugger.Remote.LogsTFApi;
    using LogChugger.Storage;
    using LogChugger.Storage.DiskLogRepository;
    using LogChugger.Storage.MySqlMetadataRepository;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Dependency injection setup/configuration.
    /// </summary>
    internal static class DependencyContainerConfig
    {
        /// <summary>
        /// Configure dependency injection.
        /// </summary>
        /// <param name="loggerFactory">A logger factory used by each component to create a logger.</param>
        /// <param name="configuration">Configuration containing the reqired sections for the sub-components.</param>
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

            builder.RegisterType<DualLogImporter>().As<IRawLogImporter>();
            builder.RegisterInstance(configuration.GetRequiredSection(DualLogImporterSettings.SectionName)
                .Get<DualLogImporterSettings>());

            builder.RegisterType<PolicedLogsTFApiClient>().As<IRemoteLogSource>();
            builder.RegisterInstance(configuration.GetRequiredSection(PolicedLogsTFApiClientSettings.SectionName)
                .Get<PolicedLogsTFApiClientSettings>());

            builder.RegisterType<MySqlMetadataRepository>().As<IRawLogMetadataRepository>();
            builder.RegisterInstance(configuration.GetRequiredSection(MySqlMetadataRepositorySettings.SectionName)
                .Get<MySqlMetadataRepositorySettings>());

            builder.RegisterType<DiskLogRepository>().As<IRawLogRepository>();
            builder.RegisterInstance(configuration.GetRequiredSection(DiskLogRepositorySettings.SectionName)
                .Get<DiskLogRepositorySettings>());

            return builder.Build();
        }
    }
}
