// <copyright file="DualLogImporter.cs" company="CIA">
// Copyright (c) CIA. All rights reserved.
// </copyright>

namespace LogChugger.Import.DualLogImporter
{
    /// <summary>
    /// Log importer that sources logs from both the local raw log repository
    /// and the remote log source.
    /// This allows bulk importing of existing logs without having to use
    /// the remote log source.
    /// </summary>
    internal class DualLogImporter : IRawLogImporter
    {
    }
}
