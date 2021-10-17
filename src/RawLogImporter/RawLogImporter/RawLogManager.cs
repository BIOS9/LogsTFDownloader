using System;
using System.Collections.Generic;
using System.Text;

namespace RawLogManager
{
    public class RawLogManager
    {
        public readonly Configuration.Configuration configuration;

        public RawLogManager(Configuration.Configuration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async void Start()
        {

        }

        public async void Stop()
        {

        }
    }
}
