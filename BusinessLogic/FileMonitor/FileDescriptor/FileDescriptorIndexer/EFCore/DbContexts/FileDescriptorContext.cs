using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer.Common;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer.EFCore.Entities;
using BusinessLogic.FrameworkAbstractions;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer.EFCore.DbContexts
{
    public class FileDescriptorContext : DbContext
    {
        public DbSet<FileDescriptorEntity> FileDescriptors { get; set; } = null!;

        // ChangeInfoType will be an integer at the DB. See https://youtu.be/RiMoKC4jJK4 for storing as string.

        // C:\Temp\Pluralsight captures\ASP.NET Core 6 Web API Fundamentals
        //    6 Getting Acquainted with EF Core  22:46

        /*
        Migrations:
        -----------

        To add migrations, do in the Package Manager Console: Add-Migration -name RecyclerDBInitialMigrationTest -outputdir FileMonitor\FileDescriptor\FileDescriptorIndexer\EFCore\Migrations
        To remove migrations, do "Remove-Migration".
        
        System.InvalidOperationException: No database provider has been configured for this DbContext.
        A provider can be configured by overriding the 'DbContext.OnConfiguring' method or by using 'AddDbContext' on the application service
        provider. If 'AddDbContext' is used, then also ensure that your DbContext type accepts a DbContextOptions<TContext> object in
        its constructor and passes it to the base constructor for DbContext.

        */

        // Needed to be commented for migrations to work ... :(
        // Seems if not commented, it will be called when Add-Migration is called, and it will only work for an ASP.NET 'builder.Services.AddDbContext<CityInfoContext>(' call.
        public FileDescriptorContext(DbContextOptions<FileDescriptorContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO: inject instead
            var helpers = new DataBaseMethodHelpers(new DirectoryWrapper());

            optionsBuilder.UseSqlite(helpers.GetConnectionString());
            base.OnConfiguring(optionsBuilder);
        }
    }
}
