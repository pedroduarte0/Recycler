using BusinessLogic.FileMonitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace BusinessLogicTests
{
    [TestClass]
    public class FileWatcherWrapperFactoryTests
    {
        [TestMethod]
        public void Create_WhenCalled_CreatesFileWatcherWrapper()
        {
            // Arrange
            var sut = new FileWatcherWrapperFactory();

            // Act
            var instance = sut.Create("some path");

            // Assert
            instance.Should().NotBeNull();
        }
    }
}
