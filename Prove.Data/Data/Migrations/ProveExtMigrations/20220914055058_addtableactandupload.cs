using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Prove.Data.Data.Migrations.ProveExtMigrations
{
    public partial class addtableactandupload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityLog",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 40, nullable: false),
                    TrxId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(maxLength: 100, nullable: false),
                    Controller = table.Column<string>(maxLength: 40, nullable: false),
                    Method = table.Column<string>(maxLength: 40, nullable: true),
                    Status = table.Column<string>(maxLength: 10, nullable: false),
                    Info = table.Column<string>(maxLength: 400, nullable: true),
                    ErrorMessage = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileUpload",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 400, nullable: true),
                    IsDeleted = table.Column<string>(maxLength: 1, nullable: true),
                    IsActive = table.Column<string>(maxLength: 1, nullable: true),
                    BucketName = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(maxLength: 100, nullable: true),
                    Length = table.Column<long>(nullable: false),
                    TrxId = table.Column<int>(nullable: false),
                    Flag = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUpload", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLog");

            migrationBuilder.DropTable(
                name: "FileUpload");
        }
    }
}
