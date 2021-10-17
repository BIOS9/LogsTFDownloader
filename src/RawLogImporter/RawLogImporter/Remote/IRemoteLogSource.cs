using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogChugger.Remote
{
    internal interface IRemoteLogSource
    {
        /**
         * <summary>Gets latest log ID from the remote source that is not past the <paramref name="ignorePast"/> time.</summary>
         * <param name="ignorePast">Ignores any logs created past this date.</param>
         */
        public Task<int> GetLatestLogIDAsync(DateTime ignorePast);
    }
}
