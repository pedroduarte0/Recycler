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
                watcher.Changed -= new FileSystemEventHandler(OnFileWatcherChanged);
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

            fileWatcherWrapper.Changed += new FileSystemEventHandler(OnFileWatcherChanged);
        }

        private void OnFileWatcherChanged(object sender, FileSystemEventArgs e)
        {
            var changeInfoType = ChangeInfoType.Created;

            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    changeInfoType = ChangeInfoType.Created;
                    break;
                case WatcherChangeTypes.Deleted:
                    changeInfoType = ChangeInfoType.Deleted;
                    break;
                case WatcherChangeTypes.Changed:
                    // TODO: should reset age?
                    break;
                case WatcherChangeTypes.Renamed:
                    // TODO: What to do?
                    break;
                case WatcherChangeTypes.All:
                    // TODO: What to do?
                    break;
                default:
                    break;
            }

            var changeInfo = new ChangeInfo(changeInfoType, e.FullPath, e.Name);

            // TODO: Temporary, provide proper implementation (put into a thread safe queue for example)
            LastCreatedChangeInfo = changeInfo;
        }

        internal IList<string> GetMonitoredFolderPath()
        {
            // TODO: return a copy.
            return m_monitoredFolders;
        }

        //TODO: Temporary
        internal ChangeInfo LastCreatedChangeInfo { get; set; }
    }

    public class ChangeInfo
    {
        public ChangeInfoType ChangeInfoType { get; private set; }
        public string FullPath { get; private set; }
        public string Name { get; private set; }

        public ChangeInfo(ChangeInfoType changeInfoType, string fullPath, string name)
        {
            ChangeInfoType = changeInfoType;
            FullPath = fullPath;
            Name = name;
        }
    }

    public enum ChangeInfoType
    {
        Created,
        Deleted
    }
}
