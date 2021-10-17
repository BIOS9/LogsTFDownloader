using System.Collections.Generic;

namespace LogChugger.Storage
{
    /// <summary>
    /// A repository that stores metadata about logs such as log IDs, hashes, import status
    /// and information about duplicate logs.
    /// </summary>
    public interface IRawLogMetadataRepository
    {
        /// <summary>
        /// Returns all log IDs with no status from 0 (Exclusive) to <paramref name="max"/>
        /// </summary>
        /// <param name="max">Maximum log ID to include in the search.</param>
        /// <returns>List of all unassigned log IDs</returns>
        public ICollection<int> GetUnassignedLogIDs(int max);
    }
}
