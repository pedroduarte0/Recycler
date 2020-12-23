using System.Collections.Generic;
using System.IO;

namespace BusinessLogic.FileMonitor
{
    public class FileMonitor : IFileMonitor
    {
        private List<string> m_monitoredFolders;
        private readonly IStorage m_storage;
        private readonly IFileWatcherWrapperFactory m_fileWatcherWrapperFactory;
        internal readonly Dictionary<string, IFileWatcherWrapper> m_fileWatcherWrappers;    // TODO: Dispose instances

        public FileMonitor(IStorage storage, IFileWatcherWrapperFactory factory)
        {
            m_monitoredFolders = new List<string>();
            m_fileWatcherWrappers = new Dictionary<string, IFileWatcherWrapper>();
            m_storage = storage;
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
            m_storage.Save(m_monitoredFolders, "monitoresFoldersList.txt");
        }

        private void Setup(IFileWatcherWrapper fileWatcherWrapper)
        {
            fileWatcherWrapper.IncludeSubdirectories = false;
            fileWatcherWrapper.EnableRaisingEvents = true;
            fileWatcherWrapper.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                | NotifyFilters.FileName;

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

            //TODO: queue the changeInfo
        }

        internal IList<string> GetMonitoredFolderPath()
        {
            // TODO: return a copy.
            return m_monitoredFolders;
        }

        //TODO: Temporary
        internal ChangeInfo LastCreatedChangeInfo { get; set; }
    }
}
