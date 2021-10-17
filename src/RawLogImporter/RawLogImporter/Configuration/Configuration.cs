using System;
using System.Collections.Generic;
using System.Text;

namespace RawLogManager.Configuration
{
    public class Configuration
    {
        public readonly ApiConfiguration ApiConfiguration;
        public readonly ImportConfiguration ImportConfiguration;

        public Configuration(ApiConfiguration apiConfiguration, ImportConfiguration importConfiguration)
        {
            ApiConfiguration = apiConfiguration ?? throw new ArgumentNullException(nameof(apiConfiguration));
            ImportConfiguration = importConfiguration ?? throw new ArgumentNullException(nameof(importConfiguration));
        }
    }
}
