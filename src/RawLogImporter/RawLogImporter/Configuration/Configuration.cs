using System;

namespace RawLogManager.Configuration
{
    public class Configuration
    {
        public readonly ApiConfiguration apiConfiguration;
        public readonly ImportConfiguration importConfiguration;

        public Configuration(ApiConfiguration apiConfiguration, ImportConfiguration importConfiguration)
        {
            this.apiConfiguration = apiConfiguration ?? throw new ArgumentNullException(nameof(apiConfiguration));
            this.importConfiguration = importConfiguration ?? throw new ArgumentNullException(nameof(importConfiguration));
        }
    }
}
