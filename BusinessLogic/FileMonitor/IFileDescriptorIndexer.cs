namespace BusinessLogic.FileMonitor
{
    public interface IFileDescriptorIndexer
    {
        void Insert(FileDescriptor descriptor);
        void Remove(FileDescriptor descriptor);
    }
}
