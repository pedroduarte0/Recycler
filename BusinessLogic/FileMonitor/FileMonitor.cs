using System.Collections.Generic;

namespace BusinessLogic
{
    public class FileMonitor : IFileMonitor
    {
        private List<string> m_monitoredFolders;

        public FileMonitor()
        {
            m_monitoredFolders = new List<string>();
        }

        public void AddFolderForMonitoring(string path)
        {
            m_monitoredFolders.Add(path);
        }

        public void RemoveFolderForMonitoring(string path)
        {
            m_monitoredFolders.Remove(path);
        }

        internal IList<string> GetMonitoredFolderPath()
        {
            return m_monitoredFolders;
        }
    }
}
