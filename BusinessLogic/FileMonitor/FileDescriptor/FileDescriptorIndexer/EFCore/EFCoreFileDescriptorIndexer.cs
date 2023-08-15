using AutoMapper;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer.Common;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer.EFCore.DbContexts;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer.EFCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer.EFCore
{
    /// <summary>
    /// Entity Framework Core implementation of <see cref="IFileDescriptorIndexer"/> using SQLite.
    /// </summary>
    public class EFCoreFileDescriptorIndexer : IFileDescriptorIndexer
    {
        private FileDescriptorContext m_context = null!;
        private readonly Mapper m_mapper;
        private readonly IDatabaseHelper m_databaseHelper;

        public EFCoreFileDescriptorIndexer(IDatabaseHelper databaseHelper)
        {
            m_databaseHelper = databaseHelper;

            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<FileDescriptor, FileDescriptorEntity>());

            m_mapper = new Mapper(config);
        }

        // TODO: Consider to move this to the constructor and remove this method.
        public void Initialize()
        {
            string connectionString = m_databaseHelper.GetConnectionString();

            var options = new DbContextOptionsBuilder<FileDescriptorContext>()
                .UseSqlite(connectionString)
                .Options;

            m_context = new FileDescriptorContext(options);
        }

        public void Dispose()
        {
            m_context?.Dispose();
        }

        public void Insert(FileDescriptor descriptor)
        {
            FileDescriptorEntity entity = GetMappedFileDescriptorEntity(descriptor);

            // Isnt there an AddOrUpdate() method?  Below works but is iterator good?
            //  https ://learn.microsoft.com/en-us/training/modules/persist-data-ef-core/4-interacting-data

            bool exists = m_context.FileDescriptors.Any(fd =>
                fd.FullPath == descriptor.FullPath);        // It is enough to check on the primary key.

            if (exists)
            {
                m_context.FileDescriptors.Update(entity);
            }
            else
            {
                m_context.FileDescriptors.Add(entity);

            }

            m_context.SaveChanges();
        }

        public void Persist()
        {
            // Might be removed, no reason for this. Made sense for the PlainTextFileDescriptorIndexer.
            // Prefearable to use SaveChanges() after each change like is currently implemented.
        }

        public void Remove(FileDescriptor descriptor)
        {
            var entity = GetMappedFileDescriptorEntity(descriptor);

            m_context.FileDescriptors.Remove(entity);
            m_context.SaveChanges();
        }

        public ICollection<FileDescriptor> RetrieveAll()
        {
            var entities = m_context.FileDescriptors.AsNoTracking().ToList();
            return m_mapper.Map<ICollection<FileDescriptor>>(entities);
        }

        private FileDescriptorEntity GetMappedFileDescriptorEntity(FileDescriptor descriptor)
        {
            return m_mapper.Map<FileDescriptorEntity>(descriptor);
        }
    }
}
