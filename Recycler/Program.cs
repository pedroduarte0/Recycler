using BusinessLogic;
using BusinessLogic.FileMonitor;
using BusinessLogic.FileMonitor.FileDescriptor;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer;
using BusinessLogic.FrameworkAbstractions;
using System.Threading.Channels;
using Unity;

namespace Recycler
{
    internal class Program
    {
        private static FileChangeMonitor? fileMonitor;
        private static AutoResetEvent m_autoResetEvent = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            SetupControlCHandler();

            var storage = Ioc.Container.Resolve<IStorage>();
            var watcherFactory = Ioc.Container.Resolve<IFileWatcherWrapperFactory>();
            var descriptorUpdater = Ioc.Container.Resolve<IFileDescriptorUpdater>();

           using var fileMonitor = new FileChangeMonitor(
                storage,
                watcherFactory,
                descriptorUpdater);

            fileMonitor.AddFolderForMonitoring("C:\\temp");

            m_autoResetEvent.WaitOne();            
        }

        private static void SetupControlCHandler()
        {
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                m_autoResetEvent.Set();
            };
        }
    }
}