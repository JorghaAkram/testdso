using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Prove.Data.Data.Migrations.ProveExtMigrations
{
    public partial class initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Glossary",
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
                    Term = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Reference = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Glossary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Probis",
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
                    ParentProbisNumber = table.Column<string>(maxLength: 50, nullable: true),
                    ChildProbisNumber = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Under_Parent = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Probis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductOfLawSKSP",
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
                    Description = table.Column<string>(maxLength: 50, nullable: true),
                    KBO = table.Column<string>(maxLength: 10, nullable: true),
                    PositionNumber = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOfLawSKSP", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaveCode",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 5, nullable: true),
                    Description = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "STKType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STKType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplateType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegulationSKSP",
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
                    RegCategoryId = table.Column<int>(nullable: false),
                    ProductOfLawSKSPId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 5, nullable: true),
                    Number = table.Column<string>(maxLength: 5, nullable: true),
                    PositionNumber = table.Column<string>(maxLength: 10, nullable: true),
                    KBO = table.Column<string>(maxLength: 10, nullable: true),
                    SaveCodeId = table.Column<int>(nullable: true),
                    SKSPNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Title = table.Column<string>(nullable: true),
                    TmtApplies = table.Column<DateTime>(nullable: false),
                    StatusDocId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    FileId = table.Column<int>(nullable: false),
                    FileSupportId = table.Column<int>(nullable: false),
                    Year = table.Column<string>(maxLength: 5, nullable: true),
                    ConceptorId = table.Column<int>(nullable: true),
                    ConceptorDirId = table.Column<int>(nullable: true),
                    ConceptorDivId = table.Column<int>(nullable: true),
                    ExpiredDate = table.Column<DateTime>(nullable: true),
                    JointReviewDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegulationSKSP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegulationSKSP_ProductOfLawSKSP_ProductOfLawSKSPId",
                        column: x => x.ProductOfLawSKSPId,
                        principalTable: "ProductOfLawSKSP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegulationSKSP_SaveCode_SaveCodeId",
                        column: x => x.SaveCodeId,
                        principalTable: "SaveCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductOfLawSTK",
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
                    TypeId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOfLawSTK", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductOfLawSTK_STKType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "STKType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Template",
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
                    TypeId = table.Column<int>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FileType = table.Column<string>(nullable: true),
                    FileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Template", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Template_TemplateType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TemplateType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegulationSTK",
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
                    RegCategoryId = table.Column<int>(nullable: false),
                    ProductOfLawSTKId = table.Column<int>(nullable: true),
                    TypeId = table.Column<int>(nullable: true),
                    Number = table.Column<string>(maxLength: 8, nullable: true),
                    SerialNumberSTK = table.Column<string>(maxLength: 8, nullable: true),
                    RefNumber = table.Column<string>(maxLength: 50, nullable: true),
                    PositionId = table.Column<string>(maxLength: 10, nullable: true),
                    KBO = table.Column<string>(maxLength: 10, nullable: true),
                    Year = table.Column<string>(maxLength: 5, nullable: true),
                    SaveCodeId = table.Column<int>(nullable: true),
                    STKNumber = table.Column<string>(maxLength: 100, nullable: true),
                    RevisedFlag = table.Column<int>(nullable: false),
                    StatusDocId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    PositionCode = table.Column<string>(maxLength: 10, nullable: true),
                    TmtApplies = table.Column<DateTime>(nullable: false),
                    ProbisId = table.Column<int>(nullable: true),
                    ProbisNumber = table.Column<string>(maxLength: 2, nullable: true),
                    Status = table.Column<string>(nullable: true),
                    FileId = table.Column<int>(nullable: false),
                    FileSupportId = table.Column<int>(nullable: false),
                    ConceptorId = table.Column<int>(nullable: true),
                    ConceptorDirId = table.Column<int>(nullable: true),
                    ConceptorDivId = table.Column<int>(nullable: true),
                    ExpiredDate = table.Column<DateTime>(nullable: true),
                    JointReviewDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegulationSTK", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegulationSTK_Probis_ProbisId",
                        column: x => x.ProbisId,
                        principalTable: "Probis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegulationSTK_ProductOfLawSTK_ProductOfLawSTKId",
                        column: x => x.ProductOfLawSTKId,
                        principalTable: "ProductOfLawSTK",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegulationSTK_SaveCode_SaveCodeId",
                        column: x => x.SaveCodeId,
                        principalTable: "SaveCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegulationSTK_STKType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "STKType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegulationHistory",
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
                    Number = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    RegulationSKSPId = table.Column<int>(nullable: true),
                    RegulationSTKId = table.Column<int>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    FileId = table.Column<int>(nullable: false),
                    FileSupportId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegulationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegulationHistory_RegulationSKSP_RegulationSKSPId",
                        column: x => x.RegulationSKSPId,
                        principalTable: "RegulationSKSP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegulationHistory_RegulationSTK_RegulationSTKId",
                        column: x => x.RegulationSTKId,
                        principalTable: "RegulationSTK",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductOfLawSTK_TypeId",
                table: "ProductOfLawSTK",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RegulationHistory_RegulationSKSPId",
                table: "RegulationHistory",
                column: "RegulationSKSPId");

            migrationBuilder.CreateIndex(
                name: "IX_RegulationHistory_RegulationSTKId",
                table: "RegulationHistory",
                column: "RegulationSTKId");

            migrationBuilder.CreateIndex(
                name: "IX_RegulationSKSP_ProductOfLawSKSPId",
                table: "RegulationSKSP",
                column: "ProductOfLawSKSPId");

            migrationBuilder.CreateIndex(
                name: "IX_RegulationSKSP_SaveCodeId",
                table: "RegulationSKSP",
                column: "SaveCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_RegulationSTK_ProbisId",
                table: "RegulationSTK",
                column: "ProbisId");

            migrationBuilder.CreateIndex(
                name: "IX_RegulationSTK_ProductOfLawSTKId",
                table: "RegulationSTK",
                column: "ProductOfLawSTKId");

            migrationBuilder.CreateIndex(
                name: "IX_RegulationSTK_SaveCodeId",
                table: "RegulationSTK",
                column: "SaveCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_RegulationSTK_TypeId",
                table: "RegulationSTK",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_TypeId",
                table: "Template",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Glossary");

            migrationBuilder.DropTable(
                name: "RegulationHistory");

            migrationBuilder.DropTable(
                name: "Template");

            migrationBuilder.DropTable(
                name: "RegulationSKSP");

            migrationBuilder.DropTable(
                name: "RegulationSTK");

            migrationBuilder.DropTable(
                name: "TemplateType");

            migrationBuilder.DropTable(
                name: "ProductOfLawSKSP");

            migrationBuilder.DropTable(
                name: "Probis");

            migrationBuilder.DropTable(
                name: "ProductOfLawSTK");

            migrationBuilder.DropTable(
                name: "SaveCode");

            migrationBuilder.DropTable(
                name: "STKType");
        }
    }
}
