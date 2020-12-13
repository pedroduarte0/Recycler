namespace BusinessLogic.FileMonitor
{
    public interface IFileWatcherWrapperFactory
    {
        /// <summary>
        /// Create an instance of <see cref="IFileWatcherWrapper"/> for monitoring a folder.
        /// </summary>
        /// <param name="folderPath">The folder path to monitor.</param>
        /// <returns>A new instance of <see cref="IFileWatcherWrapper"/>.</returns>
        IFileWatcherWrapper Create(string folderPath);
    }
}
