using System.Linq;
using BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogicTests
{
    [TestClass]
    public class FileMonitorTests
    {
        [TestMethod]
        public void AddFolderForMonitoring_FolderPath_PathIsRemembered()
        {
            // Arrange
            string path = "path to folder";

            var fileMonitor = new FileMonitor();

            // Act
            fileMonitor.AddFolderForMonitoring(path);

            // Assert
            var knownFolders = fileMonitor.GetMonitoredFolderPath();
            Assert.AreEqual(path, knownFolders.First());
        }
    }
}
