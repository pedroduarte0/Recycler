using BusinessLogic.FileMonitor;
using BusinessLogic.FileMonitor.FileDescriptor;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer.Common;
using BusinessLogic.FrameworkAbstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BusinessLogicTests
{
    // Just a way to run and debug SQLiteFileDescriptorIndexer, not the conventional unit-test style.
    [TestClass]
    public class DebugSQLiteUnitTest
    {
        private IDatabaseHelper m_helpers = new DatabaseHelper(new DirectoryWrapper());

        [TestMethod]
        public void Call_Initialize()
        {
            // Arrange
            var indexer = new SQLiteFileDescriptorIndexer(m_helpers);

            // Act
            indexer.Initialize();

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Call_Insert()
        {
            // Arrange
            var indexer = new SQLiteFileDescriptorIndexer(m_helpers);
            indexer.Initialize();

            FileDescriptor descriptor = new FileDescriptor(ChangeInfoType.Created, "fullname", "name");
            descriptor.Age = 2;

            // Act
            indexer.Insert(descriptor);

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Call_Remove()
        {
            // Arrange
            var indexer = new SQLiteFileDescriptorIndexer(m_helpers);
            indexer.Initialize();

            FileDescriptor descriptor = new FileDescriptor(ChangeInfoType.Created, "fullname", "name");
            descriptor.Age = 2;

            indexer.Insert(descriptor);

            // Act
            indexer.Remove(descriptor);

            // Assert
            Assert.IsTrue(true);
        }
    }
}
