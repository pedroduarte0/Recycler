using BusinessLogic.FileMonitor;
using BusinessLogic.FileMonitor.FileDescriptor;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BusinessLogicTests
{
    // Just a way to run and debug SQLiteFileDescriptorIndexer, not the conventional unit-test style.
    [TestClass]
    public class DebugSQLiteUnitTest
    {
        [TestMethod]
        public void Call_Initialize()
        {
            // Arrange
            var indexer = new SQLiteFileDescriptorIndexer();

            // Act
            indexer.Initialize();

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Call_Insert()
        {
            // Arrange
            var indexer = new SQLiteFileDescriptorIndexer();
            indexer.Initialize();

            FileDescriptor descriptor = new FileDescriptor(ChangeInfoType.Created, "fullname", "name");
            descriptor.Age = 2;

            // Act
            indexer.Insert(descriptor);

            // Assert
            Assert.IsTrue(true);
        }
    }
}
