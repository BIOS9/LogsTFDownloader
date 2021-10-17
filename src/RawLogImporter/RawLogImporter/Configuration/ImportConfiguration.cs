using System;

namespace RawLogManager.Configuration
{
    public class ImportConfiguration
    {
        public readonly TimeSpan importDelay;

        /**
         * <param name="importDelay">Time between an import finishing and the next one starting.</param>
         */
        public ImportConfiguration(TimeSpan importDelay)
        {
            this.importDelay = importDelay;
        }
    }
}
