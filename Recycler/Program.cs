using BusinessLogic;
using BusinessLogic.FileMonitor;
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

            BusinessLogic.IStorage storage = new TestStorage();
            IFileWatcherWrapperFactory fileWatcherWrapperFactory = new FileWatcherWrapperFactory();

            IFileMonitor fileMonitor = new FileMonitor(storage, fileWatcherWrapperFactory);
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
