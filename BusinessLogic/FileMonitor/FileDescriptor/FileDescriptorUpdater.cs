using System;
using System.Collections.Concurrent;
using BusinessLogic.FrameworkAbstractions;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer;
using System.Linq;

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
        private BlockingCollection<ChangeInfo> m_queue = new BlockingCollection<ChangeInfo>();
        private int m_persistCounter = 0;

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
            m_queue.Add(changeInfo);
        }

        internal void QueueHandler()
        {
            while (true)
            {
                try
                {
                    var item = m_queue.Take();

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

                    PeriodicallyPersistIndexer();
                }
                catch (InvalidOperationException)
                {
                    // Queue is empty and no more items will be added.
                    break;
                }
            };
        }

        private void PeriodicallyPersistIndexer()
        {
            m_persistCounter++;
            if (m_persistCounter == 10)
            {
                m_fileDescriptorIndexer.Persist();
                m_persistCounter++;
            }
        }

        internal bool QueueHasItems()
        {
            // TODO: Use if needed
            return m_queue.Any();
        }

        internal void FinalizeQueue()
        {
            m_queue.CompleteAdding();
        }
    }
}
