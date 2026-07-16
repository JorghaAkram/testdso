using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Prove.Data.Data.Migrations.ProveExtMigrations
{
    public partial class addtableuserandmodifydatatype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ConceptorId",
                table: "RegulationSTK",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConceptorDivId",
                table: "RegulationSTK",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConceptorDirId",
                table: "RegulationSTK",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConceptorId",
                table: "RegulationSKSP",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConceptorDivId",
                table: "RegulationSKSP",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConceptorDirId",
                table: "RegulationSKSP",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "User",
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
                    IdamanId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    OrganizationName = table.Column<string>(nullable: true),
                    OrganizationParentId = table.Column<string>(nullable: true),
                    OrganizationParentName = table.Column<string>(nullable: true),
                    PositionId = table.Column<string>(nullable: true),
                    PositionName = table.Column<string>(nullable: true),
                    PositionParentId = table.Column<string>(nullable: true),
                    PositionParentName = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.AlterColumn<int>(
                name: "ConceptorId",
                table: "RegulationSTK",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ConceptorDivId",
                table: "RegulationSTK",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ConceptorDirId",
                table: "RegulationSTK",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ConceptorId",
                table: "RegulationSKSP",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ConceptorDivId",
                table: "RegulationSKSP",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ConceptorDirId",
                table: "RegulationSKSP",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
