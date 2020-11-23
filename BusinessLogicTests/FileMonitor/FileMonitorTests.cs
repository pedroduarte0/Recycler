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

            var fileMonitor = GetFileMonitor();

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

            var fileMonitor = GetFileMonitor();

            // Act
            fileMonitor.AddFolderForMonitoring(path1);
            fileMonitor.AddFolderForMonitoring(path2);

            // Assert
            var knownFolders = fileMonitor.GetMonitoredFolderPath();
            Assert.AreEqual(2, knownFolders.Count(), $"Number of folder paths is incorrect.");
            Assert.AreEqual(path1, knownFolders[0]);
            Assert.AreEqual(path2, knownFolders[1]);
        }

        [TestMethod]
        public void RemoveFolderForMonitoring_FolderPath_PathIsForgot()
        {
            // Arrange
            string path = "path";

            var fileMonitor = GetFileMonitor();
            fileMonitor.AddFolderForMonitoring(path);

            // Act
            fileMonitor.RemoveFolderForMonitoring(path);

            // Assert
            var knownFolders = fileMonitor.GetMonitoredFolderPath();
            Assert.IsFalse(knownFolders.Contains(path), $"Expected path '{path}' to be removed but it was not.");
        }

        [TestMethod]
        public void RemoveFolderForMonitoring_TwoFolderPathsRemoveOne_LeaveTheOther()
        {
            // Arrange
            string pathToKeep = "pathToKeep";
            string pathToRemove = "pathToRemove";

            var fileMonitor = GetFileMonitor();
            fileMonitor.AddFolderForMonitoring(pathToKeep);
            fileMonitor.AddFolderForMonitoring(pathToRemove);

            // Act
            fileMonitor.RemoveFolderForMonitoring(pathToRemove);

            // Assert
            var knownFolders = fileMonitor.GetMonitoredFolderPath();
            Assert.IsTrue(knownFolders.Contains(pathToKeep), $"Expected path '{pathToKeep}' to be found.");
        }

        private FileMonitor GetFileMonitor()
        {
            return new FileMonitor();
        }
    }
}
