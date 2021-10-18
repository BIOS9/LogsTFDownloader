// <copyright file="DualLogImporterSettings.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Import.DualLogImporter
{
    /// <summary>
    /// Settings for <see cref="DualLogImporterSettings"/>.
    /// </summary>
    internal class DualLogImporterSettings
    {
        /// <summary>
        /// The section name to be used in the config file.
        /// </summary>
        public const string SectionName = "DualLogImporter";

        /// <summary>
        /// Gets or sets the hash algorithm to be used when checking for duplicate logs.
        /// </summary>
        public string DuplicateCheckHashAlgorithm { get; set; }
    }
}
