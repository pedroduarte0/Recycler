using BusinessLogic;
using BusinessLogic.FileMonitor;
using BusinessLogic.FileMonitor.FileDescriptor;
using BusinessLogic.FrameworkAbstractions;
using System;
using System.Windows.Forms;
using Unity;

namespace Recycler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

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
