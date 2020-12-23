using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer
{
    public class PlainTextFileDescriptorIndexer : IFileDescriptorIndexer
    {
        private Dictionary<string, FileDescriptor> m_descriptors;

        public PlainTextFileDescriptorIndexer()
        {
            m_descriptors = new Dictionary<string, FileDescriptor>();   //TODO: Load from file.
        }

        public void Insert(FileDescriptor descriptor)
        {
            m_descriptors[GetKey(descriptor)] = descriptor;
        }

        public void Persist()
        {
            throw new NotImplementedException();
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
