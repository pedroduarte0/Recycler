using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer.EFCore.Migrations
{
    public partial class RecyclerDBInitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileDescriptors",
                columns: table => new
                {
                    FullPath = table.Column<string>(type: "TEXT", maxLength: 400, nullable: false),
                    ChangeInfoType = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Age = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDescriptors", x => x.FullPath);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileDescriptors");
        }
    }
}
