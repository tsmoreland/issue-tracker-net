using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.Issues.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    _id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DisplayId = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Reporter_UserId = table.Column<Guid>(type: "TEXT", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    Reporter_FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false, defaultValue: "Unassigned"),
                    Assignee_UserId = table.Column<Guid>(type: "TEXT", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    Assignee_FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false, defaultValue: "Unassigned"),
                    EpidId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "TEXT", nullable: false),
                    LastModifiedTime = table.Column<long>(type: "INTEGER", nullable: false),
                    _epicId = table.Column<Guid>(type: "TEXT", nullable: false),
                    _epidId = table.Column<Guid>(type: "TEXT", nullable: true),
                    _issueNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    _project = table.Column<string>(type: "TEXT", unicode: false, maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x._id);
                    table.ForeignKey(
                        name: "FK_Issues_Issues__epidId",
                        column: x => x._epidId,
                        principalTable: "Issues",
                        principalColumn: "_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Issues__epidId",
                table: "Issues",
                column: "_epidId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues__id",
                table: "Issues",
                column: "_id");

            migrationBuilder.CreateIndex(
                name: "IX_Issues__issueNumber",
                table: "Issues",
                column: "_issueNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Issues__project",
                table: "Issues",
                column: "_project");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_Priority",
                table: "Issues",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_Title",
                table: "Issues",
                column: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Issues");
        }
    }
}
