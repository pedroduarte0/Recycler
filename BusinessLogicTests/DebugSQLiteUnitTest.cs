using BusinessLogic.FileMonitor;
using BusinessLogic.FileMonitor.FileDescriptor;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer;
using Xunit;

namespace BusinessLogicTests
{
    // Just a way to run and debug SQLiteFileDescriptorIndexer, not the conventional unit-test style.
    public class DebugSQLiteUnitTest
    {
        [Fact(Skip = "dummy unit test, for manual debugging")]
        public void Call_Initialize()
        {
            // Arrange
            var indexer = new SQLiteFileDescriptorIndexer();

            // Act
            indexer.Initialize();

            // Assert
            Assert.True(true);
        }

        [Fact(Skip = "dummy unit test, for manual debugging")]
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
            Assert.True(true);
        }

        [Fact(Skip = "dummy unit test, for manual debugging")]
        public void Call_Remove()
        {
            // Arrange
            var indexer = new SQLiteFileDescriptorIndexer();
            indexer.Initialize();

            FileDescriptor descriptor = new FileDescriptor(ChangeInfoType.Created, "fullname", "name");
            descriptor.Age = 2;

            indexer.Insert(descriptor);

            // Act
            indexer.Remove(descriptor);

            // Assert
            Assert.True(true);
        }
    }
}
