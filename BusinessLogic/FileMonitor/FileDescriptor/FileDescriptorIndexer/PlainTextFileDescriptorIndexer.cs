using BusinessLogic.FrameworkAbstractions;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer
{
    public class PlainTextFileDescriptorIndexer : IFileDescriptorIndexer
    {
        private readonly ISerializer<Dictionary<string, FileDescriptor>> m_serializer;
        private readonly IStorage m_storage;
        private readonly ISystemIOFileWrapper m_systemIOFile;
        private Dictionary<string, FileDescriptor> m_descriptors;
        const string m_indexPath = "FileDescriptorIndex.json";

        public PlainTextFileDescriptorIndexer(ISerializer<Dictionary<string, FileDescriptor>> serializer,
            IStorage storage,
            ISystemIOFileWrapper systemIOFile)
        {
            m_descriptors = new Dictionary<string, FileDescriptor>();
            m_serializer = serializer;
            m_storage = storage;
            m_systemIOFile = systemIOFile;
        }

        public void Initialize()
        {
            if (m_systemIOFile.Exists(m_indexPath))
            {
                m_descriptors = m_serializer.Deserialize(m_indexPath) as Dictionary<string, FileDescriptor>;
            }
        }

        public void Insert(FileDescriptor descriptor)
        {
            m_descriptors[GetKey(descriptor)] = descriptor;
        }

        public void Persist()
        {
            string json = m_serializer.Serialize(m_descriptors);
            m_storage.Save(json, m_indexPath);
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
            return descriptor.FullPath;
        }
    }
}
