using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSphere.DataAccess.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDsaQuestionDefinition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DsaQuestionDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FunctionName = table.Column<string>(type: "text", nullable: false),
                    ReturnType = table.Column<string>(type: "text", nullable: false),
                    DsaQuestionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DsaQuestionDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DsaQuestionDefinitions_DsaQuestions_DsaQuestionId",
                        column: x => x.DsaQuestionId,
                        principalTable: "DsaQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DsaQuestionDefinitionParameteres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParameterName = table.Column<string>(type: "text", nullable: false),
                    ParameterType = table.Column<string>(type: "text", nullable: false),
                    ParameterOrder = table.Column<int>(type: "integer", nullable: false),
                    DsaQuestionDefinitionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DsaQuestionDefinitionParameteres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DsaQuestionDefinitionParameteres_DsaQuestionDefinitions_Dsa~",
                        column: x => x.DsaQuestionDefinitionId,
                        principalTable: "DsaQuestionDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DsaQuestionDefinitionParameteres_DsaQuestionDefinitionId",
                table: "DsaQuestionDefinitionParameteres",
                column: "DsaQuestionDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_DsaQuestionDefinitions_DsaQuestionId",
                table: "DsaQuestionDefinitions",
                column: "DsaQuestionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DsaQuestionDefinitionParameteres");

            migrationBuilder.DropTable(
                name: "DsaQuestionDefinitions");
        }
    }
}
