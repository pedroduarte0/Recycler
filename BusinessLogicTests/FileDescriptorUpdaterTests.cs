using BusinessLogic.FileMonitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BusinessLogicTests
{
    [TestClass]
    public class FileDescriptorUpdaterTests
    {
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
        public void QueueHandler_ChangeInfoTypeCreated_CreatesFileDescriptor()
        {
            // Arrange
            var fileDescriptorIndexer = Mock.Of<IFileDescriptorIndexer>();
            var sut = new FileDescriptorUpdater(Mock.Of<IThreadWrapper>(), fileDescriptorIndexer);

            var newFileChangeInfo = new ChangeInfo(ChangeInfoType.Created, "filepath", "name");
            sut.Enqueue(newFileChangeInfo);

            // Act
            sut.QueueHandler();

            // Assert
            Mock.Get(fileDescriptorIndexer).Verify(x => x.Add(It.IsAny<FileDescriptor>()), Times.Once);
        }
    }
}
