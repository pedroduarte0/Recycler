using System;
using System.Diagnostics;
using System.Collections.Concurrent;
using BusinessLogic.FrameworkAbstractions;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer;
using System.Linq;

namespace BusinessLogic.FileMonitor.FileDescriptor
{
    /// <summary>
    /// Handles a created changed file object (<see cref="FileDescriptor"/>) and index it.
    /// </summary>
    /// <remarks>
    /// Holds a queue of <see cref="FileDescriptor"/>. A created <see cref="FileDescriptor"/> is enqueued by <see cref="FileDescriptor"/> and processed here.
    /// Uses an instance of a <see cref="FileDescriptorIndexer"/>.
    /// </remarks>
    public class FileDescriptorUpdater : IFileDescriptorUpdater
    {
        private readonly IThreadWrapper m_threadWrapper;
        private readonly IFileDescriptorIndexer m_fileDescriptorIndexer;
        private BlockingCollection<FileDescriptor> m_queue = new BlockingCollection<FileDescriptor>();
        private int m_persistCounter = 0;

        public FileDescriptorUpdater(IThreadWrapper threadWrapper, IFileDescriptorIndexer fileDescriptorIndexer)
        {
            m_threadWrapper = threadWrapper;
            m_fileDescriptorIndexer = fileDescriptorIndexer;

            m_fileDescriptorIndexer.Initialize();

            // See also ThreadStart(https://www.geeksforgeeks.org/how-to-create-threads-in-c-sharp/
            m_threadWrapper.TaskFactoryStartNew(QueueHandler);
        }

        public void Enqueue(FileDescriptor fileDescriptor)
        {
            m_queue.Add(fileDescriptor);
        }

        internal void QueueHandler()
        {
            while (true)
            {
                try
                {
                    FileDescriptor item = m_queue.Take();

                    switch (item.ChangeInfoType)
                    {
                        case ChangeInfoType.Created:
                            m_fileDescriptorIndexer.Insert(item);
                            break;

                        case ChangeInfoType.Deleted:
                            m_fileDescriptorIndexer.Remove(item);
                            break;

                        case ChangeInfoType.Changed:
                            // TODO: should reset age?
                            break;

                        default:
                            break;
                    }
                    Debug.WriteLine($"FileDescriptorUpdater: Processed '{item.FullPath}'");

                    PeriodicallyPersistIndexer();
                }
                catch (InvalidOperationException)
                {
                    // Queue is empty and no more items will be added.
                    break;
                }
            };
        }

        // Will be obsolete when using with database storage.
        private void PeriodicallyPersistIndexer()
        {
            m_persistCounter++;
            if (m_persistCounter == 10)
            {
                m_fileDescriptorIndexer.Persist();
                m_persistCounter = 0;
            }
        }

        internal bool QueueHasItems()
        {
            return m_queue.Any();
        }

        internal void FinalizeQueue()
        {
            m_queue.CompleteAdding();
        }
    }
}
