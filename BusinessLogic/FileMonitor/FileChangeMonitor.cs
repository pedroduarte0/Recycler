using System.Diagnostics;
using BusinessLogic.FileMonitor.FileDescriptor;
using System.Collections.Generic;
using BusinessLogic.FrameworkAbstractions;
using System.IO;
using System;

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

            if (!m_fileWatcherWrappers.ContainsKey(path))
            {
                var watcher = m_fileWatcherWrapperFactory.Create();
                SetupFileWatcher(watcher, path);
                m_fileWatcherWrappers[path] = watcher;
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

        public void PersistFoldersList()
        {
            m_storage.Save(m_monitoredFolders, "monitoredFoldersList.txt");
        }

        private void SetupFileWatcher(IFileWatcherWrapper fileWatcherWrapper, string path)
        {
            fileWatcherWrapper.Path = path;

            fileWatcherWrapper.IncludeSubdirectories = false;
            fileWatcherWrapper.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                | NotifyFilters.FileName;

            fileWatcherWrapper.Created += new FileSystemEventHandler(OnFileWatcherEvent);
            fileWatcherWrapper.Changed += new FileSystemEventHandler(OnFileWatcherEvent);
            fileWatcherWrapper.Deleted += new FileSystemEventHandler(OnFileWatcherEvent);
            // TODO: Rename: example .Renamed += new RenamedEventHandler(OnRenamed);
            fileWatcherWrapper.Error += new ErrorEventHandler(OnFileWatcherError);
            
            fileWatcherWrapper.EnableRaisingEvents = true;
        }

        private static void OnFileWatcherError(object sender, ErrorEventArgs e)
        {
            //  Show that an error has been detected.
            string message = "The FileSystemWatcher has detected an error";
            Console.WriteLine(message);
            Debug.WriteLine(message);
            //  Give more information if the error is due to an internal buffer overflow.
            if (e.GetException().GetType() == typeof(InternalBufferOverflowException))
            {
                //  This can happen if Windows is reporting many file system events quickly
                //  and internal buffer of the  FileSystemWatcher is not large enough to handle this
                //  rate of events. The InternalBufferOverflowException error informs the application
                //  that some of the file system events are being lost.
                message = "The file system watcher experienced an internal buffer overflow: " + e.GetException().Message;
                Console.WriteLine(message);
                Debug.WriteLine(message);
            }
        }

        private void OnFileWatcherEvent(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine($"FileSystemEventArgs: {e.Name}, {e.ChangeType}");

            var changeInfoType = ChangeInfoType.NoOperation;

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
                    changeInfoType = ChangeInfoType.Changed;
                    break;
                case WatcherChangeTypes.Renamed:
                    // TODO: What to do?
                    break;
                default:
                    changeInfoType = ChangeInfoType.NoOperation;
                    // TODO: Consider to log.
                    break;
            }

            var fileDescriptor = new FileDescriptor.FileDescriptor(changeInfoType, e.FullPath, e.Name);

            m_descriptorUpdater.Enqueue(fileDescriptor);
        }

        internal IList<string> GetMonitoredFolderPath()
        {
            // TODO: return a copy.
            return m_monitoredFolders;
        }

        public void Dispose()
        {
            foreach (var watcher in m_fileWatcherWrappers.Values)
            {
                watcher.Dispose();
            }

            m_descriptorUpdater.Dispose();
        }
    }
}
