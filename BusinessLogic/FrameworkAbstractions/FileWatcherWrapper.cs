using BusinessLogic.FileMonitor;
using System.IO;

namespace BusinessLogic.FrameworkAbstractions
{
    public class FileWatcherWrapper : FileSystemWatcher, IFileWatcherWrapper
    {
    }
}