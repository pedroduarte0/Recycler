namespace BusinessLogic.FrameworkAbstractions
{
    public interface IFileWatcherWrapperFactory
    {
        /// <summary>
        /// Create an instance of <see cref="IFileWatcherWrapper"/> for monitoring a folder.
        /// </summary>
        /// <returns>A new instance of <see cref="IFileWatcherWrapper"/>.</returns>
        IFileWatcherWrapper Create();
    }
}
