using BusinessLogic;
using BusinessLogic.FileMonitor;
using BusinessLogic.FileMonitor.FileDescriptor;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Moq;
using System.Linq;
using System.Collections.Generic;
using BusinessLogic.FrameworkAbstractions;

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

            var serializer = GetSerializerMock();
            serializer.Setup(x => x.Serialize(It.IsAny<Dictionary<string, FileDescriptor>>()))
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
            serializer.Verify(x => x.Serialize(It.IsAny<Dictionary<string, FileDescriptor>>()), Times.Once);
            storage.Verify(x => x.Save(serializationResult, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Initialize_SerializationFileDoesntExist_DoNotDeserializeIt()
        {
            // Arrange
            var fileDoesntExistSystemIOFileWrapper = GetFileDoesntExistSystemIOFileWrapper();

            var mockSerializer = Mock.Of<ISerializer<Dictionary<string, FileDescriptor>>>();

            var sut = new IndexerBuilder()
               .With(fileDoesntExistSystemIOFileWrapper)
               .With(mockSerializer)
               .Build();

            // Act
            sut.Initialize();

            // Assert
            Mock.Get(mockSerializer)
                .Verify(x => x.Deserialize(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Initialize_WhenCalled_DeserializesIndex()
        {
            // Arrange
            var serializer = GetSerializerMock();

            var fileExistsSystemIOFileWrapper = GetFileExistsSystemIOFileWrapper();

            var sut = new IndexerBuilder()
                .With(serializer.Object)
                .With(fileExistsSystemIOFileWrapper)
                .Build();

            // Act
            sut.Initialize();

            // Assert
            serializer.Verify(x => x.Deserialize(It.IsAny<string>()));
        }

        [TestMethod]
        public void Initialize_WhenCalled_RestoresIndex()
        {
            // Arrange
            var descriptor = new FileDescriptor(ChangeInfoType.Created, "fullPath", "name");
            var index = new Dictionary<string, FileDescriptor>
            {
                ["a key"] = descriptor
            };

            var serializer = GetSerializerMock();
            serializer.Setup(x => x.Deserialize(It.IsAny<string>()))
                .Returns(index);

            var storage = new Mock<IStorage>();

            var fileExistsSystemIOFileWrapper = GetFileExistsSystemIOFileWrapper();

            var sut = new IndexerBuilder()
                .With(serializer.Object)
                .With(fileExistsSystemIOFileWrapper)
                .Build();

            sut.Initialize();

            // Act
            var descriptors = sut.RetrieveAll();

            // Assert
            Assert.AreEqual(descriptors.FirstOrDefault(), descriptor);
        }

        private static Mock<ISerializer<Dictionary<string, FileDescriptor>>> GetSerializerMock()
        {
            return new Mock<ISerializer<Dictionary<string, FileDescriptor>>>();
        }

        private static ISystemIOFileWrapper GetFileExistsSystemIOFileWrapper()
        {
            return Mock.Of<ISystemIOFileWrapper>(x =>
                            x.Exists(It.IsAny<string>()) == true);
        }

        private static ISystemIOFileWrapper GetFileDoesntExistSystemIOFileWrapper()
        {
            return Mock.Of<ISystemIOFileWrapper>(x =>
                            x.Exists(It.IsAny<string>()) == false);
        }
    }

    internal class IndexerBuilder
    {
        private IStorage m_storage;
        private ISerializer<Dictionary<string, FileDescriptor>> m_serializer;
        private ISystemIOFileWrapper m_systemIOFile;

        public IndexerBuilder()
        {
            m_storage = Mock.Of<IStorage>();
            m_serializer = Mock.Of<ISerializer<Dictionary<string, FileDescriptor>>>();
            m_systemIOFile = Mock.Of<ISystemIOFileWrapper>();
        }

        public IndexerBuilder With(ISerializer<Dictionary<string, FileDescriptor>> serializer)
        {
            m_serializer = serializer;
            return this;
        }

        public IndexerBuilder With(ISystemIOFileWrapper systemIOFile)
        {
            m_systemIOFile = systemIOFile;
            return this;
        }

        public IndexerBuilder With(IStorage storage)
        {
            m_storage = storage;
            return this;
        }

        public PlainTextFileDescriptorIndexer Build()
        {
            return new PlainTextFileDescriptorIndexer(m_serializer, m_storage, m_systemIOFile);
        }
    }
}
