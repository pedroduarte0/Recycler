using BusinessLogic;
using BusinessLogic.FrameworkAbstractions;
using System.Text;
using Xunit;
using NSubstitute;

namespace BusinessLogicTests
{
    public class FileStorageTests
    {
        [Fact]
        public void SaveCollectionStrings_OneString_SavesAsLine()
        {
            // Arrange
            var collectionWithOneString = new List<string> { "singleString" };
            string path = "aFilePath";

            var wrapperMock = Substitute.For<ISystemIOFileWrapper>();

            var sut = new FileStorage(wrapperMock);

            // Act
            sut.Save(collectionWithOneString, path);

            // Assert
            wrapperMock.Received(1).WriteAllText(path, collectionWithOneString.First() + Environment.NewLine);
        }

        [Fact]
        public void SaveCollectionStrings_TwoString_SavesThemAsLines()
        {
            // Arrange
            var strings = new List<string> { "oneString", "anotherString" };
            string path = "aFilePath";

            var wrapperMock = Substitute.For<ISystemIOFileWrapper>();

            var sut = new FileStorage(wrapperMock);

            var sb = new StringBuilder();
            foreach (string item in strings)
            {
                sb.AppendLine(item);
            }

            string expectedLines = sb.ToString();

            // Act
            sut.Save(strings, path);

            // Assert
            wrapperMock.Received(1).WriteAllText(path, expectedLines);
        }

        [Fact]
        public void SaveSingleString_OneString_SavesIt()
        {
            // Arrange
            var wrapperMock = Substitute.For<ISystemIOFileWrapper>();
            var sut = new FileStorage(wrapperMock);

            string path = "a path";
            string stringToSave = "a string";

            // Act
            sut.Save(stringToSave, path);

            // Assert
            wrapperMock.Received(1).WriteAllText(path, stringToSave);
        }
        
        [Fact]
        public void Load_SourcePath_LoadsTheString()
        {
            // Arrange
            var wrapperMock = Substitute.For<ISystemIOFileWrapper>();
            var sut = new FileStorage(wrapperMock);

            string path = "a path";

            // Act
            sut.Load(path);

            // Assert
            wrapperMock.Received(1).ReadAllText(path);
        }
    }
}
