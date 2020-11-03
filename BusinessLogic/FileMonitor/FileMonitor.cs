using System.Collections.Generic;

namespace BusinessLogic
{
    public class FileMonitor : IFileMonitor
    {
        private string m_knownFolder;

        public void AddFolderForMonitoring(string path)
        {
            m_knownFolder = path;
        }

        internal IEnumerable<string> GetMonitoredFolderPath()
        {
            return new List<string> { m_knownFolder };
        }
    }
}
