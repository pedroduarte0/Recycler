using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer
{
    public class PlainTextFileDescriptorIndexer : IFileDescriptorIndexer
    {
        private readonly ISerializer m_serializer;
        private readonly IStorage m_storage;
        private Dictionary<string, FileDescriptor> m_descriptors;

        public PlainTextFileDescriptorIndexer(ISerializer serializer, IStorage storage)
        {
            m_descriptors = new Dictionary<string, FileDescriptor>();   //TODO: Load from file.
            m_serializer = serializer;
            m_storage = storage;
        }

        public void Insert(FileDescriptor descriptor)
        {
            m_descriptors[GetKey(descriptor)] = descriptor;
        }

        public void Persist()
        {
            string json = m_serializer.Serialize(m_descriptors);
            m_storage.Save(json, "FileDescriptorIndex.json");
        }

        public void Remove(FileDescriptor descriptor)
        {
            m_descriptors.Remove(GetKey(descriptor));
        }

        public ICollection<FileDescriptor> RetrieveAll()
        {
            return m_descriptors.Values.ToList();
        }

        internal bool Exists(FileDescriptor descriptor)
        {
            return m_descriptors
                .ContainsKey(GetKey(descriptor));
        }

        private string GetKey(FileDescriptor descriptor)
        {
            return Path.Combine(descriptor.FullPath, descriptor.Name);
        }
    }
}
