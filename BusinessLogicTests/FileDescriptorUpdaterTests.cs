using BusinessLogic.FileMonitor;
using BusinessLogic.FileMonitor.FileDescriptor;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer;
using BusinessLogic.FrameworkAbstractions;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace BusinessLogicTests
{
    public class FileDescriptorUpdaterTests
    {
        [Fact]
        public void Constructor_InitializesIndexer()
        {
            // Arrange
            var fileDescriptorIndexer = Substitute.For<IFileDescriptorIndexer>();

            // Act
            var sut = new FileDescriptorUpdater(Substitute.For<IThreadWrapper>(), fileDescriptorIndexer);

            // Assert
            fileDescriptorIndexer.Received(1).Initialize();
        }

        [Fact]
        public void Constructor_CreatesQueueHandler()
        {
            // Arrange
            var threadWrapper = Substitute.For<IThreadWrapper>();

            // Act
            var sut = new FileDescriptorUpdater(threadWrapper, Substitute.For<IFileDescriptorIndexer>());

            // Assert
            threadWrapper.Received(1)
                .TaskFactoryStartNew(Arg.Is<System.Action>(x => x != null));
        }

        [Fact]
        public void Enqueue_Item_ItemQueued()
        {
            // Arrange
            var sut = new FileDescriptorUpdater(Substitute.For<IThreadWrapper>(), Substitute.For<IFileDescriptorIndexer>());

            var item = new FileDescriptor(ChangeInfoType.Created, "filepath", "name");

            // Act
            sut.Enqueue(item);

            // Assert
            sut.QueueHasItems().Should().BeTrue();
        }

        [Fact]
        public void QueueHandler_ChangeInfoTypeCreated_InsertsFileDescriptor()
        {
            // Arrange
            var fileDescriptorIndexer = Substitute.For<IFileDescriptorIndexer>();
            var sut = new FileDescriptorUpdater(Substitute.For<IThreadWrapper>(), fileDescriptorIndexer);

            var newFileChangeInfo = new FileDescriptor(ChangeInfoType.Created, "filepath", "name");

            sut.Enqueue(newFileChangeInfo);
            sut.FinalizeQueue();

            // Act
            sut.QueueHandler();     // Calling it explicitely since thread creating it is stubbed.

            // Assert
            fileDescriptorIndexer.Received(1).Insert(newFileChangeInfo);
        }

        [Fact]
        public void QueueHandler_ChangeInfoTypeDeleted_RemovesFileDescriptor()
        {
            // Arrange
            var fileDescriptorIndexer = Substitute.For<IFileDescriptorIndexer>();
            var sut = new FileDescriptorUpdater(Substitute.For<IThreadWrapper>(), fileDescriptorIndexer);

            var deletedFileChangeInfo = new FileDescriptor(ChangeInfoType.Deleted, "filepath", "name");

            sut.Enqueue(deletedFileChangeInfo);
            sut.FinalizeQueue();

            // Act
            sut.QueueHandler();

            // Assert
            fileDescriptorIndexer.Received(1).Remove(deletedFileChangeInfo);
        }
    }
}
