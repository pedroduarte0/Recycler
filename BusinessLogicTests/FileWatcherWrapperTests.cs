using BusinessLogic.FileMonitor;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

namespace BusinessLogicTests
{
    [TestClass]
    public class FileWatcherWrapperTests
    {
        [TestMethod]
        public void Constructor_WhenCalled_CreatesFileSystemWatcher()
        {
            // Arrange + act
            string someRealPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var sut = new FileWatcherWrapper(someRealPath);

            // Assert
            sut.IsFileSystemWatcherNull().Should().BeFalse();
        }
    }
}
