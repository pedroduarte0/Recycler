using System;
using System.IO;

namespace BusinessLogic.FrameworkAbstractions
{
    public interface IFileWatcherWrapper : IDisposable
    {
        bool EnableRaisingEvents { get; set; }

        bool IncludeSubdirectories { get; set; }

        NotifyFilters NotifyFilter { get; set; }

        string Path { get; set; }

        event FileSystemEventHandler Created;

        event FileSystemEventHandler Changed;

        event FileSystemEventHandler Deleted;

        event ErrorEventHandler Error;
    }
}
