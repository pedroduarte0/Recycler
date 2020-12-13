namespace BusinessLogic.FileMonitor
{
    public class FileWatcherWrapper : IFileWatcherWrapper
    {
        private readonly string m_folderPath;

        public FileWatcherWrapper(string folderPath)
        {
            m_folderPath = folderPath;
        }
    }
}