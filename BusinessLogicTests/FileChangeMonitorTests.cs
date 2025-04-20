using BusinessLogic;
using BusinessLogic.FileMonitor;
using BusinessLogic.FileMonitor.FileDescriptor;
using BusinessLogic.FrameworkAbstractions;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace BusinessLogicTests
{
    namespace xUnit
    {
        public class FileChangeMonitorTests
        {
            [Fact]
            public void AddFolderForMonitoring_FolderPath_PathIsRemembered()
            {
                // Arrange
                const string path = "path to folder";

                var sut = new FileMonitorBuilder().Build();

                // Act
                sut.AddFolderForMonitoring(path);

                // Assert
                IList<string> knownFolders = sut.GetMonitoredFolderPath();
                knownFolders.First().Should().Be(path);
            }

            [Fact]
            public void AddFolderForMonitoring_AddingTwoFolderPaths_BothAreRemembered()
            {
                // Arrange
                const string path1 = "path to folder";
                const string path2 = "another path to folder";

                var sut = new FileMonitorBuilder().Build();

                // Act
                sut.AddFolderForMonitoring(path1);
                sut.AddFolderForMonitoring(path2);

                // Assert
                IList<string> knownFolders = sut.GetMonitoredFolderPath();
                knownFolders.Should().HaveCount(2);
                knownFolders[0].Should().Be(path1);
                knownFolders[1].Should().Be(path2);
            }

            [Fact]
            public void AddFolderForMonitoring_FolderPath_CreatesFileWatcher()
            {
                // Arrange
                const string path = "path to folder";

                var factoryMock = Substitute.For<IFileWatcherWrapperFactory>();
                factoryMock.Create().Returns(Substitute.For<IFileWatcherWrapper>());

                var sut = new FileMonitorBuilder().Build();

                // Act
                sut.AddFolderForMonitoring(path);

                // Assert
                factoryMock.Received(1);
            }

            [Fact]
            public void AddFolderForMonitoring_CalledTwice_CreatesOneFileWatcher()
            {
                // Arrange 
                const string path = "path to folder";

                var factoryMock = Substitute.For<IFileWatcherWrapperFactory>();
                factoryMock.Create().Returns(Substitute.For<IFileWatcherWrapper>());

                var sut = new FileMonitorBuilder()
                    .With(factoryMock)
                    .Build();

                // Act
                sut.AddFolderForMonitoring(path);
                sut.AddFolderForMonitoring(path);

                // Assert
                factoryMock.Received(1).Create();
            }

            [Fact]
            public void AddFolderForMonitoring_InitializesFileWatcher()
            {
                // Arrange
                const string path = "path to folder";

                var fileWatcherMock = Substitute.For<IFileWatcherWrapper>();

                var factory = Substitute.For<IFileWatcherWrapperFactory>();
                factory.Create().Returns(fileWatcherMock);

                var sut = new FileMonitorBuilder()
                .With(factory)
                .Build();

                // Act
                sut.AddFolderForMonitoring(path);

                // Assert   (good practices tell to test only one thing so this could be split in a series of tests)
                fileWatcherMock.Received(1).IncludeSubdirectories = false;
                fileWatcherMock.Received(1).EnableRaisingEvents = true;
                fileWatcherMock.Received(1).NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            }

            [Fact]
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

            [Fact]
            public void RemoveFolderForMonitoring_FolderPath_PathIsForgot()
            {
                // Arrange
                const string path = "path to folder";

                var fileMonitor = new FileMonitorBuilder()
                    .Build();

                fileMonitor.AddFolderForMonitoring(path);

                // Act
                fileMonitor.RemoveFolderForMonitoring(path);

                // Assert
                IList<string> knownFolders = fileMonitor.GetMonitoredFolderPath();
                knownFolders.Should().NotContain(path);
            }

            [Fact]
            public void RemoveFolderForMonitoring_TwoFolderPathsRemoveOne_LeaveTheOther()
            {
                // Arrange
                const string pathToKeep = "path to folder";
                const string pathToRemove = "another path to a different folder";

                var fileMonitor = new FileMonitorBuilder()
                    .Build();

                fileMonitor.AddFolderForMonitoring(pathToKeep);
                fileMonitor.AddFolderForMonitoring(pathToRemove);
                
                // Act
                fileMonitor.RemoveFolderForMonitoring(pathToRemove);

                // Assert
                IList<string> knownFolders = fileMonitor.GetMonitoredFolderPath();
                knownFolders.Should().HaveCount(1);
                knownFolders.First().Should().BeEquivalentTo(pathToKeep);
            }

            [Fact]
            public void RemoveFolderForMonitoring_FolderPath_DisposesFileWatcher()
            {
                // Arrange
                string path = "path";

                IFileWatcherWrapper wrapper = Substitute.For<IFileWatcherWrapper>();
                IFileWatcherWrapperFactory wrapperFactory = Substitute.For<IFileWatcherWrapperFactory>();
                wrapperFactory.Create().Returns(wrapper);

                var sut = new FileMonitorBuilder()
                    .With(wrapperFactory)
                    .Build();

                sut.AddFolderForMonitoring(path);

                // Act
                sut.RemoveFolderForMonitoring(path);

                // Assert
                wrapper.Received(1).Dispose();
            }

            [Fact]
            public void RemoveFolderForMonitoring_FolderPath_FileWatcherIsForgot()
            {
                // Arrange
                string path = "path";

                IFileWatcherWrapper wrapper = Substitute.For<IFileWatcherWrapper>();
                IFileWatcherWrapperFactory factory = Substitute.For<IFileWatcherWrapperFactory>();
                factory.Create().Returns(wrapper);

                var sut = new FileMonitorBuilder()
                    .With(factory)
                    .Build();

                sut.AddFolderForMonitoring(path);

                // Act
                sut.RemoveFolderForMonitoring(path);

                // Assert
                sut.m_fileWatcherWrappers.Should().BeEmpty();
            }

            [Fact]
            public void PersistFolders_KnownFolder_FoldersArePersisted()
            {
                // Arrange
                string path1 = "path1";
                string path2 = "path2";

                var storage = Substitute.For<IStorage>();

                var sut = new FileMonitorBuilder()
                    .With(storage)
                    .Build();

                sut.AddFolderForMonitoring(path1);
                sut.AddFolderForMonitoring(path2);

                // Act
                sut.PersistFoldersList();

                // Assert
                storage.Received(1).Save(Arg.Any<List<string>>(), Arg.Any<string>());
            }

            [Fact]
            public void OnFileWatcherChanged_NewFile_EnqueuesFileDescriptor()
            {
                // Arrange
                const string path = "path to folder";
                const string createdFileName = "filename";

                var fileWatcher = Substitute.For<IFileWatcherWrapper>();

                var factory = Substitute.For<IFileWatcherWrapperFactory>();
                factory.Create().Returns(fileWatcher);

                var descriptorUpdater = Substitute.For<IFileDescriptorUpdater>();

                var fileMonitor = new FileMonitorBuilder()
                .With(factory)
                .With(descriptorUpdater)
                .Build();

                fileMonitor.AddFolderForMonitoring(path);

                // Act
                fileWatcher.Changed += Raise.Event<FileSystemEventHandler>(new object(), new FileSystemEventArgs(
                    changeType: WatcherChangeTypes.Created,
                    directory: path,
                    name: createdFileName));

                // Assert
                descriptorUpdater.Received(1).Enqueue(Arg.Is<FileDescriptor>(c => c.FullPath ==
                    Path.Combine(path, createdFileName)));
            }
        }

        internal class FileMonitorBuilder
        {
            private IStorage m_storage;
            private IFileWatcherWrapperFactory m_fileWatcherWrapperFactory;
            private IFileDescriptorUpdater m_descriptorUpdater;

            public FileMonitorBuilder()
            {
                m_storage = Substitute.For<IStorage>();
                m_descriptorUpdater = Substitute.For<IFileDescriptorUpdater>();
                
                m_fileWatcherWrapperFactory = Substitute.For<IFileWatcherWrapperFactory>();
                m_fileWatcherWrapperFactory.Create().Returns(Substitute.For<IFileWatcherWrapper>());
            }

            public FileChangeMonitor Build()
            {
                return new FileChangeMonitor(m_storage, m_fileWatcherWrapperFactory, m_descriptorUpdater);
            }

            public FileMonitorBuilder With(IFileWatcherWrapperFactory wrapperFactory)
            {
                m_fileWatcherWrapperFactory = wrapperFactory;
                return this;
            }

            public FileMonitorBuilder With(IStorage storage)
            {
                m_storage = storage;
                return this;
            }

            public FileMonitorBuilder With(IFileDescriptorUpdater descriptorUpdater)
            {
                m_descriptorUpdater = descriptorUpdater;
                return this;
            }
        }
    }
}