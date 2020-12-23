using System;
using System.Collections.Generic;
using System.IO;

namespace BusinessLogic.FileMonitor
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
            throw new NotImplementedException();
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
