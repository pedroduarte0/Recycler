using System;
using System.Collections.Concurrent;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer;

namespace BusinessLogic.FileMonitor.FileDescriptor
{
    /// <summary>
    /// Handles a created ChangeInfo and
    ///     - If the file change is 'new file' -> create a FileDescriptor
    ///     - If the file change is 'deleted file' -> remove its FileDescriptor
    ///     
    /// Holds a queue of ChangeInfo. A created ChangeInfo is enqueued by FileMonitor and processed here.
    /// Uses an instance of a FileDescriptor indexer.
    /// </summary>
    public class FileDescriptorUpdater : IFileDescriptorUpdater
    {
        private const int MonitorPeriod = 2000;
        private readonly IThreadWrapper m_threadWrapper;
        private readonly IFileDescriptorIndexer m_fileDescriptorIndexer;
        private ConcurrentQueue<ChangeInfo> m_queue = new ConcurrentQueue<ChangeInfo>();

        public FileDescriptorUpdater(IThreadWrapper threadWrapper, IFileDescriptorIndexer fileDescriptorIndexer)
        {
            m_threadWrapper = threadWrapper;
            m_fileDescriptorIndexer = fileDescriptorIndexer;

            // TODO: Move following calls to a FileDescriptorUpdater.Initialize()
            m_fileDescriptorIndexer.Initialize();
            // See also ThreadStart(https://www.geeksforgeeks.org/how-to-create-threads-in-c-sharp/
            m_threadWrapper.TaskFactoryStartNew(QueueHandler);
        }

        public void Enqueue(ChangeInfo changeInfo)
        {
            m_queue.Enqueue(changeInfo);
        }

        internal void QueueHandler()
        {
            while (QueueHasItems())
            {
                if (m_queue.TryDequeue(out ChangeInfo item))
                {
                    switch (item.ChangeInfoType)
                    {
                        case ChangeInfoType.Created:
                            // TODO: Replace class ChangeInfo with FileDescriptor
                            var created = new FileDescriptor(item.ChangeInfoType, item.FullPath, item.Name);
                            m_fileDescriptorIndexer.Insert(created);
                            break;

                        case ChangeInfoType.Deleted:
                            var deleted = new FileDescriptor(item.ChangeInfoType, item.FullPath, item.Name);
                            m_fileDescriptorIndexer.Remove(deleted);
                            break;

                        default:
                            break;
                    }
                    Console.WriteLine($"FileDescriptorUpdater: Processed '{item.FullPath}'");
                }
            }

            m_fileDescriptorIndexer.Persist();
            m_threadWrapper.ThreadSleep(MonitorPeriod);
        }

        internal bool QueueHasItems()
        {
            return m_queue.IsEmpty == false;
        }
    }
}
