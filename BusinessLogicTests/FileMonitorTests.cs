using System.Collections.Generic;
using System.Linq;
using BusinessLogic.FileMonitor;
using FluentAssertions;
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
            const string path = "path to folder";

            var fileMonitor = new FileMonitorBuilder().Build();

            // Act
            fileMonitor.AddFolderForMonitoring(path);

            // Assert
            IList<string> knownFolders = fileMonitor.GetMonitoredFolderPath();
            knownFolders.First().Should().Be(path);
        }

        [TestMethod]
        public void AddFolderForMonitoring_AddingTwoFolderPaths_BothAreRemembered()
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
            knownFolders.Should().HaveCount(2);
            path1.Should().Be(knownFolders[0]);
            path2.Should().Be(knownFolders[1]);
        }

        [TestMethod]
        public void AddFolderForMonitoring_FolderPath_CreatesFileWatcher()
        {
            // Arrange
            const string path = "path to folder";

            var factory = Mock.Of<IFileWatcherWrapperFactory>();

            var fileMonitor = new FileMonitorBuilder()
                .WithFileWatcherWrapperFactory(factory)
                .Build();

            // Act
            fileMonitor.AddFolderForMonitoring(path);

            // Assert
            Mock.Get(factory)
                .Verify(x => x.Create(path), Times.Once);
        }

        [TestMethod]
        public void AddFolderForMonitoring_CalledTwice_CreatesOneFileWatcher()
        {
            // Arrange
            const string path = "path to folder";

            var factory = Mock.Of<IFileWatcherWrapperFactory>();

            var fileMonitor = new FileMonitorBuilder()
                .WithFileWatcherWrapperFactory(factory)
                .Build();

            // Act
            fileMonitor.AddFolderForMonitoring(path);
            fileMonitor.AddFolderForMonitoring(path);

            // Assert
            Mock.Get(factory)
                .Verify(x => x.Create(path), Times.Once);
        }

        [TestMethod]
        public void AddFolderForMonitoring_FolderPath_FileWatcherIsKeptInMemory()
        {
            // Arrange
            const string path = "path to folder";

            var fileMonitor = new FileMonitorBuilder()
                .Build();

            fileMonitor.AddFolderForMonitoring(path);

            // Act
            fileMonitor.AddFolderForMonitoring(path);

            // Assert
            var wrappers = fileMonitor.m_fileWatcherWrappers;

            wrappers.Should().HaveCount(1);
            wrappers.Should().ContainKey(path);
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
            knownFolders.Should().NotContain(path);
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
        public void RemoveFolderForMonitoring_FolderPath_DisposesFileWatcher()
        {
            // Arrange
            string path = "path";

            var wrapper = Mock.Of<IFileWatcherWrapper>();

            var wrapperFactory = Mock.Of<IFileWatcherWrapperFactory>(
                f => f.Create(It.IsAny<string>()) == wrapper);

            var fileMonitor = new FileMonitorBuilder()
                .WithFileWatcherWrapperFactory(wrapperFactory)
                .Build();

            fileMonitor.AddFolderForMonitoring(path);

            // Act
            fileMonitor.RemoveFolderForMonitoring(path);

            // Assert
            Mock.Get(wrapper).Verify(m => m.Dispose(), Times.Once);
        }

        [TestMethod]
        public void RemoveFolderForMonitoring_FolderPath_FileWatcherIsForgot()
        {
            // Arrange
            string path = "path";

            var wrapper = Mock.Of<IFileWatcherWrapper>();

            var wrapperFactory = Mock.Of<IFileWatcherWrapperFactory>(
                f => f.Create(It.IsAny<string>()) == wrapper);

            var fileMonitor = new FileMonitorBuilder()
                .WithFileWatcherWrapperFactory(wrapperFactory)
                .Build();

            fileMonitor.AddFolderForMonitoring(path);

            // Act
            fileMonitor.RemoveFolderForMonitoring(path);

            // Assert
            var wrappers = fileMonitor.m_fileWatcherWrappers;

            wrappers.Should().BeEmpty();
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
    }

    internal class FileMonitorBuilder
    {
        private Mock<IStringListPersister> m_stringListPersisterMock;
        private IFileWatcherWrapperFactory m_fileWatcherWrapperFactory;

        public FileMonitorBuilder()
        {
            m_stringListPersisterMock = new Mock<IStringListPersister>();

            m_fileWatcherWrapperFactory = Mock.Of<IFileWatcherWrapperFactory>(
                f => f.Create(It.IsAny<string>()) == Mock.Of<IFileWatcherWrapper>());
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
