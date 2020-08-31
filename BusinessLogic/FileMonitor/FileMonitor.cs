using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BusinessLogicTests")]

namespace BusinessLogic.FileMonitor
{
    public class FileMonitor : IFileMonitor
    {
        public void AddFolderForMonitoring(string path)
        {
            throw new NotImplementedException();
        }

        internal IEnumerable<string> GetMonitoredFolderPath()
        {
            throw new NotImplementedException();
        }
    }
}
