using System;
using System.IO;

namespace BusinessLogic.FileMonitor
{
    public interface IFileWatcherWrapper : IDisposable
    {
        bool EnableRaisingEvents { get; set; }

        bool IncludeSubdirectories { get; set; }

        bool IsFileSystemWatcherNull();

        NotifyFilters NotifyFilter { get; set; }

        event FileSystemEventHandler Changed;
    }
}
