using BusinessLogic;
using BusinessLogic.FileMonitor;
using BusinessLogic.FileMonitor.FileDescriptor;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer;
using BusinessLogic.FrameworkAbstractions;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

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

            // Framework dependencies
            var systemIOFile = new SystemIOFileWrapper();

            IStorage storage = new TestStorage();
            IThreadWrapper threadWrapper = new ThreadWrapper();

            IFileWatcherWrapperFactory watcherFactory = new FileWatcherWrapperFactory();

            IFileDescriptorIndexer descriptorIndexer = new PlainTextFileDescriptorIndexer(
                new JsonSerializer<Dictionary<string, FileDescriptor>>(systemIOFile),       // will be prettier once using IoC framework
                storage,
                systemIOFile);

            IFileDescriptorUpdater descriptorUpdater = new FileDescriptorUpdater(threadWrapper, descriptorIndexer);

            IFileChangeMonitor fileMonitor = new FileChangeMonitor(
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

    public class TestStorage : BusinessLogic.IStorage
    {
        public void Save(List<string> strings, string filePath)
        {
        }

        public void Save(string singleToSave, string filePath)
        {
        }
    }
}
