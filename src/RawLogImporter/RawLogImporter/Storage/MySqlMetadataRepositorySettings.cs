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
        public const string SectionName = "MySqlMetadataRepository";

        public string ConnectionString { get; set; }
    }
}
