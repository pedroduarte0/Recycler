using BusinessLogic.FileMonitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Linq;

namespace BusinessLogicTests
{
    [TestClass]
    public class PlainTextFileDescriptorIndexerTests
    {
        [TestMethod]
        public void Insert_FileDescriptor_Inserted()
        {
            // Arrange
            var sut = new PlainTextFileDescriptorIndexer();

            var fd = new FileDescriptor(ChangeInfoType.Created, "apath", "name");

            // Act
            sut.Insert(fd);

            // Assert
            sut.Exists(fd).Should().BeTrue();
        }

        [TestMethod]
        public void RetrieveAll_TwoFileDescriptors_Retrieved()
        {
            // Arrange
            var sut = new PlainTextFileDescriptorIndexer();

            var fd1 = new FileDescriptor(ChangeInfoType.Created, "apath", "name");
            sut.Insert(fd1);

            var fd2 = new FileDescriptor(ChangeInfoType.Created, "anotherPath", "anotherName");
            sut.Insert(fd2);

            // Act
            var allDescriptors = sut.RetrieveAll();

            // Assert
            sut.Exists(fd1).Should().BeTrue();
            sut.Exists(fd2).Should().BeTrue();
        }

        [TestMethod]
        public void Insert_FileDescriptorExists_IsUpdated()
        {
            // Arrange
            var sut = new PlainTextFileDescriptorIndexer();

            var fd = new FileDescriptor(ChangeInfoType.Created, "apath", "name");
            sut.Insert(fd);

            fd = sut.RetrieveAll().First();
            int originalAge = fd.Age;

            fd.Age++;
            int expectedAge = fd.Age;

            // Act
            sut.Insert(fd);

            // Assert
            sut.Exists(fd).Should().BeTrue();
            sut.RetrieveAll().First().Age.Should().Be(expectedAge);
        }

        [TestMethod]
        public void Remove_FileDescriptor_Removed()
        {
            // Arrange
            var sut = new PlainTextFileDescriptorIndexer();

            var fd = new FileDescriptor(ChangeInfoType.Created, "apath", "name");
            sut.Insert(fd);

            // Act
            sut.Remove(fd);

            // Assert
            sut.Exists(fd).Should().BeFalse();
        }
    }
}
