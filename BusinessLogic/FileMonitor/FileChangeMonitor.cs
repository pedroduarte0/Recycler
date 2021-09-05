using BusinessLogic.FileMonitor.FileDescriptor;
using System.Collections.Generic;
using BusinessLogic.FrameworkAbstractions;
using System.IO;

namespace BusinessLogic.FileMonitor
{
    public class FileChangeMonitor : IFileChangeMonitor
    {
        private List<string> m_monitoredFolders;
        private readonly IStorage m_storage;
        private readonly IFileWatcherWrapperFactory m_fileWatcherWrapperFactory;
        private readonly IFileDescriptorUpdater m_descriptorUpdater;
        internal readonly Dictionary<string, IFileWatcherWrapper> m_fileWatcherWrappers;    // TODO: Confirm instances are being disposed.

        public FileChangeMonitor(IStorage storage, IFileWatcherWrapperFactory factory, IFileDescriptorUpdater descriptorUpdater)
        {
            m_monitoredFolders = new List<string>();
            m_fileWatcherWrappers = new Dictionary<string, IFileWatcherWrapper>();
            m_storage = storage;
            m_fileWatcherWrapperFactory = factory;
            m_descriptorUpdater = descriptorUpdater;
        }

        public void AddFolderForMonitoring(string path)
        {
            m_monitoredFolders.Add(path);

            if (m_fileWatcherWrappers.ContainsKey(path) == false)
            {
                var fileWatcherWrapper = m_fileWatcherWrapperFactory.Create();
                Setup(fileWatcherWrapper, path);
                m_fileWatcherWrappers[path] = fileWatcherWrapper;
            }
        }

        public void RemoveFolderForMonitoring(string path)
        {
            m_monitoredFolders.Remove(path);

            if (m_fileWatcherWrappers.ContainsKey(path))
            {
                var watcher = m_fileWatcherWrappers[path];
                watcher.Changed -= new FileSystemEventHandler(OnFileWatcherEvent);
                watcher.Created -= new FileSystemEventHandler(OnFileWatcherEvent);
                watcher.Deleted -= new FileSystemEventHandler(OnFileWatcherEvent);
                watcher.Dispose();
                m_fileWatcherWrappers.Remove(path);
            }
        }

        public void PersistFolders()
        {
            m_storage.Save(m_monitoredFolders, "monitoresFoldersList.txt");
        }

        private void Setup(IFileWatcherWrapper fileWatcherWrapper, string path)
        {
            fileWatcherWrapper.Path = path;

            fileWatcherWrapper.IncludeSubdirectories = false;
            fileWatcherWrapper.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                | NotifyFilters.FileName;

            fileWatcherWrapper.Created += new FileSystemEventHandler(OnFileWatcherEvent);
            fileWatcherWrapper.Changed += new FileSystemEventHandler(OnFileWatcherEvent);
            fileWatcherWrapper.Deleted += new FileSystemEventHandler(OnFileWatcherEvent);
            fileWatcherWrapper.EnableRaisingEvents = true;
        }

        private void OnFileWatcherEvent(object sender, FileSystemEventArgs e)
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

            m_descriptorUpdater.Enqueue(changeInfo);
        }

        internal IList<string> GetMonitoredFolderPath()
        {
            // TODO: return a copy.
            return m_monitoredFolders;
        }
    }
}
