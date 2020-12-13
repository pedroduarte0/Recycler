using System;
using System.IO;

namespace BusinessLogic.FileMonitor
{
    public interface IFileWatcherWrapper : IDisposable
    {
        bool IsFileSystemWatcherNull();
    }
}
