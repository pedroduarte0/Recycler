using BusinessLogic;
using BusinessLogic.FileMonitor;
using BusinessLogic.FileMonitor.FileDescriptor;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer;
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

            IStorage storage = new TestStorage();
            IFileWatcherWrapperFactory fileWatcherWrapperFactory = new FileWatcherWrapperFactory();
            IThreadWrapper threadWrapper = null;
            IFileDescriptorIndexer descriptorIndexer = null;
            IFileDescriptorUpdater descriptorUpdater = new FileDescriptorUpdater(threadWrapper, descriptorIndexer);

            IFileMonitor fileMonitor = new FileMonitor(storage, fileWatcherWrapperFactory, descriptorUpdater);
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
