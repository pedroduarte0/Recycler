using BusinessLogic.FileMonitor;
using BusinessLogic.FileMonitor.FileDescriptor;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer;
using BusinessLogic.FrameworkAbstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BusinessLogicTests
{
    [TestClass]
    public class FileDescriptorUpdaterTests
    {
        [TestMethod]
        public void Constructor_InitializesIndexer()
        {
            // Arrange
            var fileDescriptorIndexer = new Mock<IFileDescriptorIndexer>();

            // Act
            var sut = new FileDescriptorUpdater(Mock.Of<IThreadWrapper>(), fileDescriptorIndexer.Object);

            // Assert
            fileDescriptorIndexer.Verify(x => x.Initialize(), Times.Once());
        }

        [TestMethod]
        public void Constructor_CreatesQueueHandler()
        {
            // Arrange
            var threadWrapper = new Mock<IThreadWrapper>();

            // Act
            var sut = new FileDescriptorUpdater(threadWrapper.Object, Mock.Of<IFileDescriptorIndexer>());

            // Assert
            threadWrapper.Verify(x =>
                x.TaskFactoryStartNew(It.IsAny<System.Action>()), Times.Once);
        }

        [TestMethod]
        public void Enqueue_Item_ItemQueued()
        {
            // Arrange
            var sut = new FileDescriptorUpdater(Mock.Of<IThreadWrapper>(), Mock.Of<IFileDescriptorIndexer>());

            var item = new ChangeInfo(ChangeInfoType.Created, "filepath", "name");

            // Act
            sut.Enqueue(item);

            // Assert
            Assert.IsTrue(sut.QueueHasItems());
        }

        [TestMethod]
        public void QueueHandler_ChangeInfoTypeCreated_InsertsFileDescriptor()
        {
            // Arrange
            var fileDescriptorIndexer = Mock.Of<IFileDescriptorIndexer>();
            var sut = new FileDescriptorUpdater(Mock.Of<IThreadWrapper>(), fileDescriptorIndexer);

            var newFileChangeInfo = new ChangeInfo(ChangeInfoType.Created, "filepath", "name");
            sut.Enqueue(newFileChangeInfo);

            // Act
            sut.QueueHandler();

            // Assert
            Mock.Get(fileDescriptorIndexer)
                .Verify(x => x.Insert(
                    It.Is<FileDescriptor>(fd => fd.ChangeInfoType == ChangeInfoType.Created)),
                    Times.Once);
        }

        [TestMethod]
        public void QueueHandler_ChangeInfoTypeDeleted_RemovesFileDescriptor()
        {
            // Arrange
            var fileDescriptorIndexer = Mock.Of<IFileDescriptorIndexer>();
            var sut = new FileDescriptorUpdater(Mock.Of<IThreadWrapper>(), fileDescriptorIndexer);

            var deletedFileChangeInfo = new ChangeInfo(ChangeInfoType.Deleted, "filepath", "name");
            sut.Enqueue(deletedFileChangeInfo);

            // Act
            sut.QueueHandler();

            // Assert
            Mock.Get(fileDescriptorIndexer)
                .Verify(x => x.Remove(
                    It.Is<FileDescriptor>(fd => fd.ChangeInfoType == ChangeInfoType.Deleted)),
                    Times.Once);
        }
    }
}
