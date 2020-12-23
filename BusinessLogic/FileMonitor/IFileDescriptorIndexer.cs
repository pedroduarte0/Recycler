namespace BusinessLogic.FileMonitor
{
    public interface IFileDescriptorIndexer
    {
        void Insert(FileDescriptor descriptor);
        void Remove(FileDescriptor descriptor);
        void Persist();     // Might be removed in the future, once implementation is for a database indexer.
    }
}
