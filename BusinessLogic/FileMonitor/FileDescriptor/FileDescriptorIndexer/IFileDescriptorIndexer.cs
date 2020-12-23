using System.Collections.Generic;

namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer
{
    public interface IFileDescriptorIndexer
    {
        void Insert(FileDescriptor descriptor);

        ICollection<FileDescriptor> RetrieveAll();

        void Remove(FileDescriptor descriptor);

        void Persist();     // Might be removed in the future, once implementation is for a database indexer.
    }
}
