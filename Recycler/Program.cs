using BusinessLogic;
using BusinessLogic.FileMonitor;
using BusinessLogic.FileMonitor.FileDescriptor;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer;
using BusinessLogic.FrameworkAbstractions;
using Unity;

namespace Recycler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var storage = Ioc.Container.Resolve<IStorage>();
            var watcherFactory = Ioc.Container.Resolve<IFileWatcherWrapperFactory>();
            var descriptorUpdater = Ioc.Container.Resolve<IFileDescriptorUpdater>();

            var fileMonitor = new FileChangeMonitor(
                storage,
                watcherFactory,
                descriptorUpdater);

            fileMonitor.AddFolderForMonitoring("C:\\temp");

            while (true)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}