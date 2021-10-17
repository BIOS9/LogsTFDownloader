using System.Collections.Generic;

namespace LogChugger.Storage
{
    internal class MySqlMetadataRepository : IRawLogMetadataRepository
    {
        public ICollection<int> GetUnassignedLogIDs(int max)
        {
            throw new System.NotImplementedException();
        }
    }
}
