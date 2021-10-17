using System.Collections.Generic;

namespace LogChugger.Storage
{
    /// <summary>
    /// A MySql based metadata storage repository for raw log metadata.
    /// </summary>
    internal class MySqlMetadataRepository : IRawLogMetadataRepository
    {
        /// <inheritdoc/>
        public ICollection<int> GetUnassignedLogIDs(int max)
        {
            throw new System.NotImplementedException();
        }
    }
}
