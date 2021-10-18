// <copyright file="MySqlMetadataRepository.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Storage.MySqlMetadataRepository
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A MySql based metadata storage repository for raw log metadata.
    /// </summary>
    internal class MySqlMetadataRepository : IRawLogMetadataRepository
    {
        private readonly MySqlMetadataRepositorySettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlMetadataRepository"/> class.
        /// </summary>
        /// <param name="settings">MySql settings.</param>
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
