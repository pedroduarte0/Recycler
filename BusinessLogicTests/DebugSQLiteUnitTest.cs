using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BusinessLogicTests
{
    // Just some tests to run and debug SQLiteFileDescriptorIndexer
    [TestClass]
    public class DebugSQLiteUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var indexer = new SQLiteFileDescriptorIndexer();

            // Act
            indexer.Initialize();

            // Assert
            Assert.IsTrue(true);
        }
    }
}
