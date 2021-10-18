namespace LogChugger.Storage
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A MySql based metadata storage repository for raw log metadata.
    /// </summary>
    internal class MySqlMetadataRepository : IRawLogMetadataRepository
    {
        private readonly MySqlMetadataRepositorySettings settings;

        public MySqlMetadataRepository(MySqlMetadataRepositorySettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }



        /// <inheritdoc/>
        public ICollection<int> GetUnassignedLogIDs(int max)
        {
            throw new System.NotImplementedException();
        }
    }
}
