using System.Collections.Generic;

namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer
{
    public interface IFileDescriptorIndexer
    {
        void Initialize();

        void Insert(FileDescriptor descriptor);

        ICollection<FileDescriptor> RetrieveAll();

        void Remove(FileDescriptor descriptor);

        void Persist();     // Might be removed in the future, once implementation is for a database indexer, as only needed for file storage indexer PlainTextFileDescriptorIndexer.
                            // Should not be part of the interface since doesnt make sense for a DB: Either remove together with PlainTextFileDescriptorIndexer or perhaps create an adapter?
    }
}
