using System.IO;

namespace BusinessLogic.FileMonitor
{
    public class FileWatcherWrapper : FileSystemWatcher, IFileWatcherWrapper
    {
        private FileSystemWatcher m_fileSystemWatcher;

        public FileWatcherWrapper(string folderPath)
        {
            m_fileSystemWatcher = new FileSystemWatcher(folderPath);
        }

        public bool IsFileSystemWatcherNull()
        {
            return m_fileSystemWatcher == null;
        }

        new public void Dispose()   //TODO: Dispose is not implemented properly.
        {
            m_fileSystemWatcher.Dispose();
        }
    }
}