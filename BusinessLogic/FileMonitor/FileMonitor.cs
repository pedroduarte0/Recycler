using System.Collections.Generic;
using System.IO;

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
                var fileWatcherWrapper = m_fileWatcherWrapperFactory.Create(path);
                Setup(fileWatcherWrapper);
                m_fileWatcherWrappers[path] = fileWatcherWrapper;
            }
        }

        public void RemoveFolderForMonitoring(string path)
        {
            m_monitoredFolders.Remove(path);

            if (m_fileWatcherWrappers.ContainsKey(path))
            {
                var watcher = m_fileWatcherWrappers[path];
                watcher.Changed -= new FileSystemEventHandler(OnChanged);
                watcher.Dispose();
                m_fileWatcherWrappers.Remove(path);
            }
        }

        public void PersistFolders()
        {
            m_persister.Persist(m_monitoredFolders);
        }

        private void Setup(IFileWatcherWrapper fileWatcherWrapper)
        {
            fileWatcherWrapper.IncludeSubdirectories = false;
            fileWatcherWrapper.EnableRaisingEvents = true;
            fileWatcherWrapper.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            fileWatcherWrapper.Changed += new FileSystemEventHandler(OnChanged);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
        }

        internal IList<string> GetMonitoredFolderPath()
        {
            // TODO: return a copy.
            return m_monitoredFolders;
        }
    }
}
