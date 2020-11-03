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

        [TestMethod]
        public void AddFolderForMonitoring_AdddingTwoFolderPaths_BothAreRemembered()
        {
            // Arrange
            string path1 = "path1";
            string path2 = "path2";

            var fileMonitor = new FileMonitor();

            // Act
            fileMonitor.AddFolderForMonitoring(path1);
            fileMonitor.AddFolderForMonitoring(path2);

            // Assert
            var knownFolders = fileMonitor.GetMonitoredFolderPath();
            Assert.AreEqual(2, knownFolders.Count(), $"Number of folder paths is incorrect.");
            Assert.AreEqual(path1, knownFolders[0]);
            Assert.AreEqual(path2, knownFolders[1]);
        }
    }
}
