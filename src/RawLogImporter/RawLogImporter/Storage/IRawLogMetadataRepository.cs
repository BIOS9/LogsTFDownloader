using System.Collections.Generic;

namespace LogChugger.Storage
{
    public interface IRawLogMetadataRepository
    {
        /**
         * <summary>Returns all log IDs with no status from 0 (Exclusive) to <paramref name="max"/></summary>
         */
        public ICollection<int> GetUnassignedLogIDs(int max);
    }
}
