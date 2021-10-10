using BusinessLogic;
using BusinessLogic.FrameworkAbstractions;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;

namespace BusinessLogicTests
{
    [TestClass]
    public class FileStorageTests
    {
        [TestMethod]
        public void SaveCollectionStrings_OneString_SavesAsLine()
        {
            // Arrange
            var collectionWithOneString = new List<string> { "singleString" };
            string path = "aFilePath";

            var wrapperMock = Mock.Of<ISystemIOFileWrapper>();
            
            var sot = new FileStorage(wrapperMock);

            // Act
            sot.Save(collectionWithOneString, path);

            // Assert
            Mock.Get(wrapperMock)
                .Verify(x => x.WriteAllText(path, collectionWithOneString.First() + Environment.NewLine), Times.Once);
        }

        [TestMethod]
        public void SaveCollectionStrings_TwoString_SavesThemAsLines()
        {
            // Arrange
            var strings = new List<string> { "oneString", "anotherString" };
            string path = "aFilePath";

            var wrapperMock = Mock.Of<ISystemIOFileWrapper>();

            var sot = new FileStorage(wrapperMock);

            var sb = new StringBuilder();
            foreach (string item in strings)
            {
                sb.AppendLine(item);
            }

            string expectedLines = sb.ToString();

            // Act
            sot.Save(strings, path);

            // Assert
            Mock.Get(wrapperMock)
                .Verify(x => x.WriteAllText(path, expectedLines), Times.Once);
        }

        [TestMethod]
        public void SaveSingleStrings_OneString_SavesIt()
        {
            // Arrange
            string oneString = "singleString";
            string path = "aFilePath";

            var wrapperMock = Mock.Of<ISystemIOFileWrapper>();

            var sot = new FileStorage(wrapperMock);

            // Act
            sot.Save(oneString, path);

            // Assert
            Mock.Get(wrapperMock)
                .Verify(x => x.WriteAllText(path, oneString), Times.Once);
        }

        [TestMethod]
        public void Load_SourcePath_LoadsTheString()
        {
            // Arrange
            string path = "aFilePath";

            var wrapperMock = Mock.Of<ISystemIOFileWrapper>();

            var sot = new FileStorage(wrapperMock);

            // Act
            sot.Load(path);

            // Assert
            Mock.Get(wrapperMock)
                .Verify(x => x.ReadAllText(path), Times.Once);
        }
    }
}
