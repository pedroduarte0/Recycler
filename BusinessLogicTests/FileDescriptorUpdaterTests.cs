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
            var sut = new FileDescriptorUpdater(threadWrapper.Object);

            // Assert
            threadWrapper.Verify(x =>
                x.TaskFactoryStartNew(It.IsAny<System.Action>()), Times.Once);
        }

        [TestMethod]
        public void Enqueue_Item_ItemQueued()
        {
            // Arrange
            var threadWrapper = new Mock<IThreadWrapper>();
            var sut = new FileDescriptorUpdater(threadWrapper.Object);

            var item = new ChangeInfo(ChangeInfoType.Created, "filepath", "name");

            // Act
            sut.Enqueue(item);

            // Assert
            Assert.IsTrue(sut.QueueHasItems());
        }
    }
}
