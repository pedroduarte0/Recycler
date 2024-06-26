﻿using System.Collections.Generic;
using Unity;
using BusinessLogic;
using BusinessLogic.FrameworkAbstractions;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer;
using BusinessLogic.FileMonitor.FileDescriptor;

namespace Recycler
{
    public static class Ioc
    {
        public static IUnityContainer Container => m_container;

        private static readonly IUnityContainer m_container;

        static Ioc()
        {
            m_container = new UnityContainer();

            m_container.RegisterType<IStorage, DummyStorage>(
                TypeLifetime.Singleton);

            m_container.RegisterType<ISystemIOFileWrapper, SystemIOFileWrapper>(
                TypeLifetime.Singleton);

            m_container.RegisterType<IThreadWrapper, ThreadWrapper>(
                TypeLifetime.Singleton);

            m_container.RegisterType<IFileWatcherWrapperFactory, FileWatcherWrapperFactory>(
                TypeLifetime.Singleton);

            m_container.RegisterType<IFileDescriptorIndexer, PlainTextFileDescriptorIndexer>(
                TypeLifetime.Singleton);

            m_container.RegisterType<IFileDescriptorUpdater, FileDescriptorUpdater>(
                TypeLifetime.Singleton);

            m_container.RegisterType<IFileChangeMonitor, IFileChangeMonitor>(
                TypeLifetime.Singleton);

            m_container.RegisterType<ISerializer<Dictionary<string, FileDescriptor>>, JsonSerializer<Dictionary<string, FileDescriptor>>>(
                TypeLifetime.Singleton);
        }

        public class DummyStorage : IStorage
        {
            private SystemIOFileWrapper systemIOFile = new SystemIOFileWrapper();

            public string Load(string source)
            {
                return systemIOFile.ReadAllText(source);
            }

            public void Save(ICollection<string> strings, string filePath)
            {
            }

            public void Save(string singleToSave, string filePath)
            {
                systemIOFile.WriteAllText(filePath, singleToSave);
            }
        }
    }
}
