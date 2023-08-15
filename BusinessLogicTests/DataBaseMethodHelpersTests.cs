using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer.Common;
using BusinessLogic.FrameworkAbstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BusinessLogicTests
{
    [TestClass]
    public class DatabaseHelperTests
    {
        [TestMethod]
        public void GetConnectionString_CreatesDbPath_IfNotExists()
        {
            // Arrange
            var input = new List<string>();

            // Setup the Exists method and record its input.
            var directoryWrapper = Mock.Of<IDirectoryWrapper>(x =>
                x.Exists(Capture.In(input)) == false);

            DatabaseHelper sut = new(directoryWrapper);

            // Act
            string actual = sut.GetConnectionString();

            // Assert
            Mock.Get(directoryWrapper)
                .Verify(x => x.CreateDirectory(input.First()), Times.Once);
        }
    }
}
