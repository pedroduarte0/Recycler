using BusinessLogic.FileMonitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace BusinessLogicTests
{
    [TestClass]
    public class PlainTextFileDescriptorIndexerTests
    {
        [TestMethod]
        public void Insert_FileDescriptor_Exists()
        {
            // Arrange
            var sut = new PlainTextFileDescriptorIndexer();

            var fd = new FileDescriptor(ChangeInfoType.Created, "apath", "name");

            // Act
            sut.Insert(fd);

            // Assert
            sut.Exists(fd).Should().BeTrue();
        }
    }
}
