using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSphere.DataAccess.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Service : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DsaQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionTitle = table.Column<string>(type: "text", nullable: false),
                    QuestionDescription = table.Column<string>(type: "text", nullable: false),
                    DifficultyLevel = table.Column<string>(type: "text", nullable: false),
                    SolutionUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DsaQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DsaTopics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TopicName = table.Column<string>(type: "text", nullable: false),
                    TopicLevel = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DsaTopics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DsaQuestionTestCases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Input = table.Column<string>(type: "text", nullable: false),
                    ExpectedOutput = table.Column<string>(type: "text", nullable: false),
                    IsHidden = table.Column<bool>(type: "boolean", nullable: false),
                    DsaQuestionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DsaQuestionTestCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DsaQuestionTestCases_DsaQuestions_DsaQuestionId",
                        column: x => x.DsaQuestionId,
                        principalTable: "DsaQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DsaTopicQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DsaTopicsId = table.Column<Guid>(type: "uuid", nullable: false),
                    DsaQuestionsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DsaTopicQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DsaTopicQuestions_DsaQuestions_DsaQuestionsId",
                        column: x => x.DsaQuestionsId,
                        principalTable: "DsaQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DsaTopicQuestions_DsaTopics_DsaTopicsId",
                        column: x => x.DsaTopicsId,
                        principalTable: "DsaTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DsaQuestionTestCases_DsaQuestionId",
                table: "DsaQuestionTestCases",
                column: "DsaQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_DsaTopicQuestions_DsaQuestionsId",
                table: "DsaTopicQuestions",
                column: "DsaQuestionsId");

            migrationBuilder.CreateIndex(
                name: "IX_DsaTopicQuestions_DsaTopicsId",
                table: "DsaTopicQuestions",
                column: "DsaTopicsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DsaQuestionTestCases");

            migrationBuilder.DropTable(
                name: "DsaTopicQuestions");

            migrationBuilder.DropTable(
                name: "DsaQuestions");

            migrationBuilder.DropTable(
                name: "DsaTopics");
        }
    }
}
