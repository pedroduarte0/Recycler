namespace BusinessLogic.FrameworkAbstractions
{
    public class FileWatcherWrapperFactory : IFileWatcherWrapperFactory
    {
        public IFileWatcherWrapper Create()
        {
            return new FileWatcherWrapper();
        }
    }
}
