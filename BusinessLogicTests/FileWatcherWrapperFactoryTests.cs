﻿using BusinessLogic.FileMonitor;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

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
            string someRealPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Act
            var instance = sut.Create(someRealPath);

            // Assert
            instance.Should().NotBeNull();
        }
    }
}