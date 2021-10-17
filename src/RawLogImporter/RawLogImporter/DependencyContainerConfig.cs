using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using LogChugger.Remote;
using LogChugger.Remote.LogsTFApi;
using LogChugger.Storage;

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
            builder.RegisterType<PolicedLogsTFApiClient>().As<IRemoteLogSource>();
            builder.RegisterType<MySqlMetadataRepository>().As<IRawLogMetadataRepository>();
            builder.RegisterType<DiskLogRepository>().As<IRawLogRepository>();


            return builder.Build();
        }
    }
}
