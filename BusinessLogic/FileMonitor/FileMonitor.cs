using System.Collections.Generic;

namespace BusinessLogic.FileMonitor
{
    public class FileMonitor : IFileMonitor
    {
        private List<string> m_monitoredFolders;
        private readonly IStringListPersister m_persister;
        private readonly IFileWatcherWrapperFactory m_fileWatcherWrapperFactory;
        internal readonly Dictionary<string, IFileWatcherWrapper> m_fileWatcherWrappers;    // TODO: Dispose instances

        public FileMonitor(IStringListPersister persister, IFileWatcherWrapperFactory factory)
        {
            m_monitoredFolders = new List<string>();
            m_fileWatcherWrappers = new Dictionary<string, IFileWatcherWrapper>();
            m_persister = persister;
            m_fileWatcherWrapperFactory = factory;
        }

        public void AddFolderForMonitoring(string path)
        {
            m_monitoredFolders.Add(path);

            if (m_fileWatcherWrappers.ContainsKey(path) == false)
            {
                var instance = m_fileWatcherWrapperFactory.Create(path);
                m_fileWatcherWrappers[path] = instance;
            }
        }

        public void RemoveFolderForMonitoring(string path)
        {
            m_monitoredFolders.Remove(path);

            if (m_fileWatcherWrappers.ContainsKey(path))
            {
                var watcher = m_fileWatcherWrappers[path];
                watcher.Dispose();
                m_fileWatcherWrappers.Remove(path);
            }
        }

        internal IList<string> GetMonitoredFolderPath()
        {
            // TODO: return a copy.
            return m_monitoredFolders;
        }

        public void PersistFolders()
        {
            m_persister.Persist(m_monitoredFolders);
        }
    }
}
