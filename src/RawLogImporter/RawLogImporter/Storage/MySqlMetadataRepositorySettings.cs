// <copyright file="MySqlMetadataRepositorySettings.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Storage
{
    /// <summary>
    /// Settings for <see cref="MySqlMetadataRepository"/>.
    /// </summary>
    internal class MySqlMetadataRepositorySettings
    {
        /// <summary>
        /// The section name to be used in the config file.
        /// </summary>
        public const string SectionName = "MySqlMetadataRepository";

        /// <summary>
        /// Gets or sets the MySql connection string used to access the database server.
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
