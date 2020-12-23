using BusinessLogic;
using BusinessLogic.FileMonitor;
using BusinessLogic.FileMonitor.FileDescriptor;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Moq;
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
            var sut = new IndexerBuilder().Build();

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
            var sut = new IndexerBuilder().Build();

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
            var sut = new IndexerBuilder().Build();

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
            var sut = new IndexerBuilder().Build();

            var fd = new FileDescriptor(ChangeInfoType.Created, "apath", "name");
            sut.Insert(fd);

            // Act
            sut.Remove(fd);

            // Assert
            sut.Exists(fd).Should().BeFalse();
        }

        [TestMethod]
        public void Persist_WhenCalled_SerializesAndSavesDescriptors()
        {
            // Arrange
            const string serializationResult = "json string";

            var serializer = new Mock<ISerializer>();
            serializer.Setup(x => x.Serialize(It.IsAny<object>()))
                .Returns(serializationResult);

            var storage = new Mock<IStorage>();

            var sut = new IndexerBuilder()
                .With(serializer.Object)
                .With(storage.Object)
                .Build();

            var fd = new FileDescriptor(ChangeInfoType.Created, "apath", "name");
            sut.Insert(fd);

            // Act
            sut.Persist();

            // Assert
            serializer.Verify(x => x.Serialize(It.IsAny<object>()), Times.Once);
            storage.Verify(x => x.Save(serializationResult, It.IsAny<string>()), Times.Once);
        }
    }

    internal class IndexerBuilder
    {
        private IStorage m_storage;
        private ISerializer m_serializer;

        public IndexerBuilder()
        {
            m_storage = Mock.Of<IStorage>();
            m_serializer = Mock.Of<ISerializer>();
        }

        public IndexerBuilder With(ISerializer serializer)
        {
            m_serializer = serializer;
            return this;
        }

        public IndexerBuilder With(IStorage storage)
        {
            m_storage = storage;
            return this;
        }

        public PlainTextFileDescriptorIndexer Build()
        {
            return new PlainTextFileDescriptorIndexer(m_serializer, m_storage);
        }
    }
}
