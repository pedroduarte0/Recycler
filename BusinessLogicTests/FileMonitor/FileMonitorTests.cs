using System.Collections.Generic;
using System.Linq;
using BusinessLogic.FileMonitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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

            var fileMonitor = new FileMonitorBuilder().Build();

            // Act
            fileMonitor.AddFolderForMonitoring(path);

            // Assert
            IList<string> knownFolders = fileMonitor.GetMonitoredFolderPath();
            Assert.AreEqual(path, knownFolders.First());
        }

        [TestMethod]
        public void AddFolderForMonitoring_AdddingTwoFolderPaths_BothAreRemembered()
        {
            // Arrange
            string path1 = "path1";
            string path2 = "path2";

            var fileMonitor = new FileMonitorBuilder().Build();

            // Act
            fileMonitor.AddFolderForMonitoring(path1);
            fileMonitor.AddFolderForMonitoring(path2);

            // Assert
            IList<string> knownFolders = fileMonitor.GetMonitoredFolderPath();
            Assert.AreEqual(2, knownFolders.Count(), $"Number of folder paths is incorrect.");
            Assert.AreEqual(path1, knownFolders[0]);
            Assert.AreEqual(path2, knownFolders[1]);
        }

        [TestMethod]
        public void RemoveFolderForMonitoring_FolderPath_PathIsForgot()
        {
            // Arrange
            string path = "path";

            var fileMonitor = new FileMonitorBuilder().Build();
            fileMonitor.AddFolderForMonitoring(path);

            // Act
            fileMonitor.RemoveFolderForMonitoring(path);

            // Assert
            IList<string> knownFolders = fileMonitor.GetMonitoredFolderPath();
            Assert.IsFalse(knownFolders.Contains(path), $"Expected path '{path}' to be removed but it was not.");
        }

        [TestMethod]
        public void RemoveFolderForMonitoring_TwoFolderPathsRemoveOne_LeaveTheOther()
        {
            // Arrange
            string pathToKeep = "pathToKeep";
            string pathToRemove = "pathToRemove";

            var fileMonitor = new FileMonitorBuilder().Build();
            fileMonitor.AddFolderForMonitoring(pathToKeep);
            fileMonitor.AddFolderForMonitoring(pathToRemove);

            // Act
            fileMonitor.RemoveFolderForMonitoring(pathToRemove);

            // Assert
            IList<string> knownFolders = fileMonitor.GetMonitoredFolderPath();
            Assert.IsTrue(knownFolders.Contains(pathToKeep), $"Expected path '{pathToKeep}' to be found.");
        }

        [TestMethod]
        public void PersistFolders_KnownFolder_FoldersArePersisted()
        {
            // Arrange
            string path1 = "path1";
            string path2 = "path2";

            var stringListPersisterMock = new Mock<IStringListPersister>();

            var fileMonitor = new FileMonitorBuilder()
                .With(stringListPersisterMock)
                .Build();

            fileMonitor.AddFolderForMonitoring(path1);
            fileMonitor.AddFolderForMonitoring(path2);

            // Act
            fileMonitor.PersistFolders();

            // Assert
            stringListPersisterMock.Verify(x => x.Persist(It.IsAny<IList<string>>()));

        }

        [TestMethod]
        public void NewFile_FileDescriptorCreated()
        {
            // todo: FileMonitor seems a FileSystemWatcherWrapperBuilder: we need one FileSystemWatcher per known folder
            // (btw, know folders are recursive, so known folder A cannot be inside of a known folder B) therefore we need a builder)
            // the FileSystemWatcherWrapper wraps to the .NET FileSystemWatcher as in https://stackoverflow.com/questions/33254493/unit-testing-filesystemwatcher-how-to-programatically-fire-a-changed-event
        }

        [TestMethod]
        public void StartMonitoring_OneMonitoredFolder_CreatesOneFileWatcher()
        {
            // Arrange
            var factory = Mock.Of<IFileWatcherWrapperFactory>();

            var fileMonitor = new FileMonitorBuilder()
                .WithFileWatcherWrapperFactory(factory)
                .Build();

            fileMonitor.AddFolderForMonitoring("path to folder");

            // Act
            fileMonitor.StartMonitoring();

            // Assert
            Mock.Get(factory)
                .Verify(x => x.Create("path to folder"), Times.Once);
        }

        [TestMethod]
        public void StartMonitoring_TwoMonitoredFolders_CreatesTwoFileWatcher()
        {
            // Arrange
            var factory = Mock.Of<IFileWatcherWrapperFactory>();

            var fileMonitor = new FileMonitorBuilder()
                .WithFileWatcherWrapperFactory(factory)
                .Build();

            fileMonitor.AddFolderForMonitoring("path1");
            fileMonitor.AddFolderForMonitoring("path2");

            // Act
            fileMonitor.StartMonitoring();

            // Assert
            Mock.Get(factory)
                .Verify(x => x.Create("path1"), Times.Once);

            Mock.Get(factory)
                .Verify(x => x.Create("path2"), Times.Once);
        }

        [TestMethod]
        public void StartMonitoring_OneFileWatcherCreated_InstanceKeptInMemory()
        {
            // Arrange
            var factory = Mock.Of<IFileWatcherWrapperFactory>();

            var fileMonitor = new FileMonitorBuilder()
                .WithFileWatcherWrapperFactory(factory)
                .Build();

            fileMonitor.AddFolderForMonitoring("path");

            // Act
            fileMonitor.StartMonitoring();

            // Assert
            Assert.IsTrue(fileMonitor.m_fileWatcherWrappers.ContainsKey("path"));
        }
    }

    internal class FileMonitorBuilder
    {
        private Mock<IStringListPersister> m_stringListPersisterMock;
        private IFileWatcherWrapperFactory m_fileWatcherWrapperFactory;

        public FileMonitorBuilder()
        {
            m_stringListPersisterMock = new Mock<IStringListPersister>();
        }

        public FileMonitorBuilder With(Mock<IStringListPersister> mock)
        {
            m_stringListPersisterMock = mock;
            return this;
        }

        public FileMonitorBuilder WithFileWatcherWrapperFactory(IFileWatcherWrapperFactory factory)
        {
            m_fileWatcherWrapperFactory = factory;
            return this;
        }

        public FileMonitor Build()
        {
            return new FileMonitor(m_stringListPersisterMock.Object, m_fileWatcherWrapperFactory);
        }
    }
}
