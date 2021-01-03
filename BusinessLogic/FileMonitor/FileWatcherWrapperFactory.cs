namespace BusinessLogic.FileMonitor
{
    public class FileWatcherWrapperFactory : IFileWatcherWrapperFactory
    {
        public IFileWatcherWrapper Create()
        {
            return new FileWatcherWrapper();
        }
    }
}
