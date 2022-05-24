namespace BusinessLogic.FileMonitor.FileDescriptor
{
    public interface IFileDescriptorUpdater
    {
        void Enqueue(FileDescriptor fileDescriptor);
    }
}
