using Microsoft.EntityFrameworkCore.Migrations;

namespace Prove.Data.Data.Migrations.ProveExtMigrations
{
    public partial class addcolumncocode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "Template",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "RegulationSTK",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "RegulationSKSP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "ProductOfLawSTK",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "ProductOfLawSKSP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "Glossary",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "RegulationSTK");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "RegulationSKSP");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "ProductOfLawSTK");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "ProductOfLawSKSP");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "Glossary");
        }
    }
}
