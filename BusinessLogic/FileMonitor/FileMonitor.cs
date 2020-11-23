using System;
using System.Collections.Generic;

namespace BusinessLogic.FileMonitor
{
    public class FileMonitor : IFileMonitor
    {
        private List<string> m_monitoredFolders;
        private readonly IStringListPersister m_persister;

        public FileMonitor(IStringListPersister persister)
        {
            m_monitoredFolders = new List<string>();
            m_persister = persister;
        }

        public void AddFolderForMonitoring(string path)
        {
            m_monitoredFolders.Add(path);
        }

        internal IList<string> GetMonitoredFolderPath()
        {
            return m_monitoredFolders;
        }

        public void RemoveFolderForMonitoring(string path)
        {
            m_monitoredFolders.Remove(path);
        }

        public void PersistFolders()
        {
            m_persister.Persist(m_monitoredFolders);
        }
    }
}
