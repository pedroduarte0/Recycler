namespace BusinessLogic.FileMonitor.FileDescriptor
{
    public interface IFileDescriptorUpdater : IDisposable    {
        void Enqueue(FileDescriptor fileDescriptor);
    }
}
