namespace BusinessLogic.FileMonitor
{
    public class FileWatcherWrapperFactory : IFileWatcherWrapperFactory
    {
        public IFileWatcherWrapper Create(string folderPath)
        {
            return new FileWatcherWrapper(folderPath);
        }
    }
}
