namespace BusinessLogic.FileMonitor
{
    public interface IFileDescriptorIndexer
    {
        void Add(FileDescriptor descriptor);
        void Remove(FileDescriptor descriptor);
    }
}
