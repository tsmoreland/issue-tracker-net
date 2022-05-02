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
                    IssueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Project = table.Column<string>(type: "TEXT", unicode: false, maxLength: 3, nullable: false),
                    IssueNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Assignee_UserId = table.Column<Guid>(type: "TEXT", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    Assignee_FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false, defaultValue: "Unassigned"),
                    Reporter_UserId = table.Column<Guid>(type: "TEXT", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    Reporter_FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false, defaultValue: "Unassigned"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.IssueId);
                });

            migrationBuilder.CreateTable(
                name: "LinkedIssues",
                columns: table => new
                {
                    ParentIssueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChildIssueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LinkType = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<string>(type: "TEXT", nullable: false),
                    ChildId = table.Column<string>(type: "TEXT", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkedIssues", x => new { x.ParentIssueId, x.ChildIssueId });
                    table.ForeignKey(
                        name: "FK_LinkedIssues_Issues_ChildIssueId",
                        column: x => x.ChildIssueId,
                        principalTable: "Issues",
                        principalColumn: "IssueId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LinkedIssues_Issues_ParentIssueId",
                        column: x => x.ParentIssueId,
                        principalTable: "Issues",
                        principalColumn: "IssueId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("1642a18b-96a1-46f0-a834-5fcb72e14aaa"), "e86ac0ac-80ba-4108-8aa3-7265c8240a68", "As a user I want to be able to retreive all projects", "APP-3", 3, 637766379000000000L, 0, "APP", "The api should be able to retrieve projects", 1 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("9776c88f-d2cc-4e01-8d34-56710cbd2be4"), "4446f14e-71e1-415a-aab4-78bdae749755", "", "APP-2", 2, 637766376000000000L, 1, "APP", "The database should story issues with links to projects", 1 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("b3941d54-f1f9-4b90-9e84-c7f33cee6d97"), "b32115cc-d27b-4aeb-b9fc-5a50f6b4ae33", "add the request handlers to get projects by id and a summary", "APP-5", 5, 637768143000000000L, 0, "APP", "add mediator request/handlers for project query", 2 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("ef0b233b-bed3-4027-9ed9-010fe00ecf2f"), "474effa9-b4a4-4a0b-ad02-2d2c5490aad1", "add the model(s) for project type ensuring it's id matches the expectations of issue", "APP-4", 4, 637767216000000000L, 1, "APP", "add project core models", 2 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("fb648532-026f-447c-a9c7-996fac6a8b2e"), "bad845f6-cdb9-4148-b24c-051d783796ff", "add projects and use project id as link between issue and project", "APP-1", 1, 637766370000000000L, 2, "APP", "Add Project support", 0 });

            migrationBuilder.InsertData(
                table: "LinkedIssues",
                columns: new[] { "ChildIssueId", "ParentIssueId", "ChildId", "ConcurrencyToken", "LinkType", "ParentId" },
                values: new object[] { new Guid("1642a18b-96a1-46f0-a834-5fcb72e14aaa"), new Guid("9776c88f-d2cc-4e01-8d34-56710cbd2be4"), "APP-3", "95904269-574f-4135-9ed2-3abd801e15e9", 0, "APP-2" });

            migrationBuilder.InsertData(
                table: "LinkedIssues",
                columns: new[] { "ChildIssueId", "ParentIssueId", "ChildId", "ConcurrencyToken", "LinkType", "ParentId" },
                values: new object[] { new Guid("b3941d54-f1f9-4b90-9e84-c7f33cee6d97"), new Guid("ef0b233b-bed3-4027-9ed9-010fe00ecf2f"), "APP-5", "198375d6-fc95-4d80-92e1-96f7c1438d58", 2, "APP-4" });

            migrationBuilder.CreateIndex(
                name: "IX_Issues_Id",
                table: "Issues",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_IssueNumber",
                table: "Issues",
                column: "IssueNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_Priority",
                table: "Issues",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_Project",
                table: "Issues",
                column: "Project");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_Title",
                table: "Issues",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_LinkedIssues_ChildId",
                table: "LinkedIssues",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkedIssues_ChildIssueId",
                table: "LinkedIssues",
                column: "ChildIssueId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkedIssues_ParentId",
                table: "LinkedIssues",
                column: "ParentId");
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
