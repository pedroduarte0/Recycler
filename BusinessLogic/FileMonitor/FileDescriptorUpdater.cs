using System;
using System.Collections.Concurrent;

namespace BusinessLogic.FileMonitor
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
        static ConcurrentQueue<ChangeInfo> m_queue = new ConcurrentQueue<ChangeInfo>();

        public FileDescriptorUpdater(IThreadWrapper threadWrapper, IFileDescriptorIndexer fileDescriptorIndexer)
        {
            m_threadWrapper = threadWrapper;
            m_fileDescriptorIndexer = fileDescriptorIndexer;

            // See also ThreadStart(https://www.geeksforgeeks.org/how-to-create-threads-in-c-sharp/
            m_threadWrapper.TaskFactoryStartNew(QueueHandler);    //TODO: Abstract to wrapper in order to be testable.
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
                    //TODO if change is new, create FileDescriptor and m_fileDescriptorIndexer.Add(descriptor)
                    //     if change is deleted, m_fileDescriptorIndexer.Remove(descriptor)

                    Console.WriteLine($"FileDescriptorUpdater: Processed '{item.FullPath}'");
                }
            }

            m_threadWrapper.ThreadSleep(MonitorPeriod);
        }

        internal bool QueueHasItems()
        {
            return m_queue.IsEmpty == false;
        }
    }
}
