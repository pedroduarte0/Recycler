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
        static ConcurrentQueue<ChangeInfo> m_queue = new ConcurrentQueue<ChangeInfo>();

        public FileDescriptorUpdater(IThreadWrapper threadWrapper)
        {
            m_threadWrapper = threadWrapper;

            // See also ThreadStart(https://www.geeksforgeeks.org/how-to-create-threads-in-c-sharp/
            m_threadWrapper.TaskFactoryStartNew(QueueHandler);    //TODO: Abstract to wrapper in order to be testable.
        }

        public void Enqueue(ChangeInfo changeInfo)
        {
            m_queue.Enqueue(changeInfo);
        }

        private void QueueHandler()
        {
            bool run = true;
            while (run)
            {
                ChangeInfo item;

                while (QueueHasItems() && run)
                {
                    if (m_queue.TryDequeue(out item) && run)
                    {

                        Console.WriteLine($"FileDescriptorUpdater: Processed '{item.FullPath}'");
                    }
                }

                m_threadWrapper.ThreadSleep(MonitorPeriod);
            }
        }

        private static bool QueueHasItems()
        {
            return m_queue.IsEmpty == false;
        }
    }
}
