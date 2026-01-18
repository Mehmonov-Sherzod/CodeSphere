using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSphere.DataAccess.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDsaQuestionDefinitionSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DsaQuestionDefinitions_DsaQuestions_DsaQuestionId",
                table: "DsaQuestionDefinitions");

            migrationBuilder.DropColumn(
                name: "ParameterOrder",
                table: "DsaQuestionDefinitionParameteres");

            migrationBuilder.RenameColumn(
                name: "FunctionName",
                table: "DsaQuestionDefinitions",
                newName: "MethodName");

            migrationBuilder.RenameColumn(
                name: "DsaQuestionId",
                table: "DsaQuestionDefinitions",
                newName: "DsaQuestionsId");

            migrationBuilder.RenameIndex(
                name: "IX_DsaQuestionDefinitions_DsaQuestionId",
                table: "DsaQuestionDefinitions",
                newName: "IX_DsaQuestionDefinitions_DsaQuestionsId");

            migrationBuilder.RenameColumn(
                name: "ParameterType",
                table: "DsaQuestionDefinitionParameteres",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "ParameterName",
                table: "DsaQuestionDefinitionParameteres",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "DsaQuestionDefinitions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_DsaQuestionDefinitions_DsaQuestions_DsaQuestionsId",
                table: "DsaQuestionDefinitions",
                column: "DsaQuestionsId",
                principalTable: "DsaQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DsaQuestionDefinitions_DsaQuestions_DsaQuestionsId",
                table: "DsaQuestionDefinitions");

            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "DsaQuestionDefinitions");

            migrationBuilder.RenameColumn(
                name: "MethodName",
                table: "DsaQuestionDefinitions",
                newName: "FunctionName");

            migrationBuilder.RenameColumn(
                name: "DsaQuestionsId",
                table: "DsaQuestionDefinitions",
                newName: "DsaQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_DsaQuestionDefinitions_DsaQuestionsId",
                table: "DsaQuestionDefinitions",
                newName: "IX_DsaQuestionDefinitions_DsaQuestionId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "DsaQuestionDefinitionParameteres",
                newName: "ParameterType");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DsaQuestionDefinitionParameteres",
                newName: "ParameterName");

            migrationBuilder.AddColumn<int>(
                name: "ParameterOrder",
                table: "DsaQuestionDefinitionParameteres",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_DsaQuestionDefinitions_DsaQuestions_DsaQuestionId",
                table: "DsaQuestionDefinitions",
                column: "DsaQuestionId",
                principalTable: "DsaQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
