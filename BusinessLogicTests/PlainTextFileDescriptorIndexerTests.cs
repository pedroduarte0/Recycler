using BusinessLogic;
using BusinessLogic.FileMonitor;
using BusinessLogic.FileMonitor.FileDescriptor;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer;
using FluentAssertions;
using BusinessLogic.FrameworkAbstractions;
using Xunit;
using NSubstitute;

namespace BusinessLogicTests
{
    public class PlainTextFileDescriptorIndexerTests
    {
        [Fact]
        public void Insert_FileDescriptor_Inserted()
        {
            // Arrange
            var sut = new IndexerBuilder().Build();

            FileDescriptor fd = new(ChangeInfoType.Created, "fullpath", "name");

            // Act
            sut.Insert(fd);

            // Assert
            sut.Exists(fd).Should().BeTrue();
        }

        [Fact]
        public void RetrieveAll_TwoFileDescriptors_Retrieved()
        {
            // Arrange
            var sut = new IndexerBuilder().Build();

            FileDescriptor fd1 = new(ChangeInfoType.Created, "fullpath1", "name1");
            sut.Insert(fd1);

            FileDescriptor fd2 = new(ChangeInfoType.Changed, "fullpath2", "name2");
            sut.Insert(fd2);

            // Act
            var result = sut.RetrieveAll().ToList();

            // Assert
            result.Count.Should().Be(2);
            result[0].Should().Be(fd1);
            result[1].Should().Be(fd2);
        }

        [Fact]
        public void Insert_FileDescriptorExists_IsUpdated()
        {
            // Arrange
            var sut = new IndexerBuilder().Build();

            FileDescriptor fd = new(ChangeInfoType.Created, "path", "name");

            sut.Insert(fd);

            fd = sut.RetrieveAll().First();

            fd.Age++;
            int expectedAge = fd.Age;

            // Act
            sut.Insert(fd);

            // Assert
            sut.Exists(fd).Should().BeTrue();
            sut.RetrieveAll().First().Age.Should().Be(expectedAge);
        }

        [Fact]
        public void Remove_FileDescriptor_Removed()
        {
            // Arrange
            var sut = new IndexerBuilder().Build();

            var fd = new FileDescriptor(ChangeInfoType.Created, "path", "name");
            sut.Insert(fd);

            // Act
            sut.Remove(fd);

            // Assert
            sut.Exists(fd).Should().BeFalse();
        }

        [Fact]
        public void Persist_WhenCalled_SerializesAndSavesDescriptors()
        {
            // Arrange
            var storage = Substitute.For<IStorage>();
            var serializer = Substitute.For<ISerializer<Dictionary<string, FileDescriptor>>>();

            var sut = new IndexerBuilder()
                .WithStorage(storage)
                .WithSerializer(serializer)
                .Build();

            // Act
            sut.Persist();

            // Assert
            storage.Received(1).Save(Arg.Any<string>(), Arg.Any<string>());
            serializer.Received(1).Serialize(Arg.Any<Dictionary<string, FileDescriptor>>());
        }

        [Fact]
        public void Initialize_SerializationFileDoesntExist_DoNotDeserializeIt()
        {
            // Arrange
            var fileDoesntExistSystemIOFileWrapper = GetFileDoesntExistSystemIOFileWrapper();

            var mockSerializer = Substitute.For<ISerializer<Dictionary<string, FileDescriptor>>>();

            var sut = new IndexerBuilder()
               .WithSystemIOFileWrapper(fileDoesntExistSystemIOFileWrapper)
               .WithSerializer(mockSerializer)
               .Build();

            // Act
            sut.Initialize();

            // Assert
            mockSerializer.Received(0).Deserialize(Arg.Any<string>());
        }

        [Fact]
        public void Initialize_WhenCalled_DeserializesIndex()
        {
            // Arrange
            var fileExistsSystemIOFileWrapper = GetFileExistsSystemIOFileWrapper();
            
            const string deserializedIndex = "test";
            fileExistsSystemIOFileWrapper.ReadAllText(Arg.Any<string>()).Returns(deserializedIndex);

            var serializer = Substitute.For<ISerializer<Dictionary<string, FileDescriptor>>>();

            var sut = new IndexerBuilder()
                .WithSystemIOFileWrapper(fileExistsSystemIOFileWrapper)
                .WithSerializer(serializer)
                .Build();

            // Act
            sut.Initialize();

            // Assert
            serializer.Received(1).Deserialize(deserializedIndex);
        }

        [Fact]
        public void Initialize_WhenCalled_RestoresIndex()
        {
            // Arrange
            var descriptor = new FileDescriptor(ChangeInfoType.Created, "fullPath", "name");
            var index = new Dictionary<string, FileDescriptor>
            {
                ["a key"] = descriptor
            };

            var serializer = Substitute.For<ISerializer<Dictionary<string, FileDescriptor>>>();
            serializer.Deserialize(Arg.Any<string>()).Returns(index);

            var fileExistsSystemIOFileWrapper = GetFileExistsSystemIOFileWrapper();

            var sut = new IndexerBuilder()
                .WithSerializer(serializer)
                .WithSystemIOFileWrapper(fileExistsSystemIOFileWrapper)
                .Build();

            // Act
            sut.Initialize();

            // Assert
            var descriptors = sut.RetrieveAll();
            descriptors.First().Should().BeSameAs(descriptor);
        }

        private static ISystemIOFileWrapper GetFileDoesntExistSystemIOFileWrapper()
        {
            var mock = Substitute.For<ISystemIOFileWrapper>();
            mock.Exists(Arg.Any<string>()).Returns(false);
            
            return mock;
        }

        private static ISystemIOFileWrapper GetFileExistsSystemIOFileWrapper()
        {
            var mock = Substitute.For<ISystemIOFileWrapper>();
            mock.Exists(Arg.Any<string>()).Returns(true);

            return mock;
        }
    }

    internal class IndexerBuilder
    {
        private ISerializer<Dictionary<string, FileDescriptor>> _serializer;
        private IStorage _storage;
        private ISystemIOFileWrapper _systemIo;

        public IndexerBuilder()
        {
            _serializer = Substitute.For<ISerializer<Dictionary<string, FileDescriptor>>>();
            _storage = Substitute.For<IStorage>();
            _systemIo = Substitute.For<ISystemIOFileWrapper>();
        }

        public IndexerBuilder WithSystemIOFileWrapper(ISystemIOFileWrapper wrapper)
        {
            _systemIo = wrapper;
            return this;
        }

        public IndexerBuilder WithStorage(IStorage storage)
        {
            _storage = storage;
            return this;
        }

        public IndexerBuilder WithSerializer(ISerializer<Dictionary<string, FileDescriptor>> serializer)
        {
            _serializer = serializer;
            return this;
        }

        public PlainTextFileDescriptorIndexer Build()
        {
            return new PlainTextFileDescriptorIndexer(_serializer, _storage, _systemIo);
        }
    }
}
