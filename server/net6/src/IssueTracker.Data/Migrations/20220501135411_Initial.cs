using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<string>(type: "TEXT", unicode: false, maxLength: 3, nullable: false),
                    IssueNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    AssigneeId = table.Column<Guid>(type: "TEXT", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    AssigneeFullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false, defaultValue: "Unassigned"),
                    ReporterId = table.Column<Guid>(type: "TEXT", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    ReporterFullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false, defaultValue: "Unassigned"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LinkedIssues",
                columns: table => new
                {
                    ParentIssueId = table.Column<string>(type: "TEXT", nullable: false),
                    ChildIssueId = table.Column<string>(type: "TEXT", nullable: false),
                    LinkType = table.Column<int>(type: "INTEGER", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkedIssues", x => new { x.ParentIssueId, x.ChildIssueId });
                    table.ForeignKey(
                        name: "FK_LinkedIssues_Issues_ChildIssueId",
                        column: x => x.ChildIssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LinkedIssues_Issues_ParentIssueId",
                        column: x => x.ParentIssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "ConcurrencyToken", "Description", "IssueNumber", "LastUpdated", "Priority", "ProjectId", "Title", "Type" },
                values: new object[] { "APP-1", "e653141e-0cf0-4da2-97be-b54e3de00cb3", "add projects and use project id as link between issue and project", 1, 637766370000000000L, 2, "APP", "Add Project support", 0 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "ConcurrencyToken", "Description", "IssueNumber", "LastUpdated", "Priority", "ProjectId", "Title", "Type" },
                values: new object[] { "APP-2", "a69efa66-c58e-4330-b37d-9796aa76a37d", "", 2, 637766376000000000L, 1, "APP", "The database should story issues with links to projects", 1 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "ConcurrencyToken", "Description", "IssueNumber", "LastUpdated", "Priority", "ProjectId", "Title", "Type" },
                values: new object[] { "APP-3", "14730f8b-c599-44d0-8a6f-b4d7e9088fa1", "As a user I want to be able to retreive all projects", 3, 637766379000000000L, 0, "APP", "The api should be able to retrieve projects", 1 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "ConcurrencyToken", "Description", "IssueNumber", "LastUpdated", "Priority", "ProjectId", "Title", "Type" },
                values: new object[] { "APP-4", "4323ddf2-77b5-4cef-b430-7590a05c933c", "add the model(s) for project type ensuring it's id matches the expectations of issue", 4, 637767216000000000L, 1, "APP", "add project core models", 2 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "ConcurrencyToken", "Description", "IssueNumber", "LastUpdated", "Priority", "ProjectId", "Title", "Type" },
                values: new object[] { "APP-5", "7d55b877-aafb-4a06-ab98-50d7e300d48a", "add the request handlers to get projects by id and a summary", 5, 637768143000000000L, 0, "APP", "add mediator request/handlers for project query", 2 });

            migrationBuilder.InsertData(
                table: "LinkedIssues",
                columns: new[] { "ChildIssueId", "ParentIssueId", "ConcurrencyToken", "LinkType" },
                values: new object[] { "APP-3", "APP-2", "b2b4646e-4512-4567-8195-23e08821e2df", 0 });

            migrationBuilder.InsertData(
                table: "LinkedIssues",
                columns: new[] { "ChildIssueId", "ParentIssueId", "ConcurrencyToken", "LinkType" },
                values: new object[] { "APP-5", "APP-4", "2b988dd1-2002-4a25-b5a0-dbbefff57af1", 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Issues_IssueNumber",
                table: "Issues",
                column: "IssueNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_Priority",
                table: "Issues",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_ProjectId",
                table: "Issues",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_Title",
                table: "Issues",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_LinkedIssues_ChildIssueId",
                table: "LinkedIssues",
                column: "ChildIssueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkedIssues");

            migrationBuilder.DropTable(
                name: "Issues");
        }
    }
}
