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
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    Reporter_UserId = table.Column<Guid>(type: "TEXT", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    Reporter_FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false, defaultValue: "Unassigned"),
                    Assignee_UserId = table.Column<Guid>(type: "TEXT", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    Assignee_FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false, defaultValue: "Unassigned"),
                    ConcurrencyToken = table.Column<string>(type: "TEXT", nullable: false),
                    EpicId = table.Column<string>(type: "TEXT", nullable: true),
                    IssueNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Project = table.Column<string>(type: "TEXT", unicode: false, maxLength: 3, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    LastModifiedTime = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Issues_Issues_EpicId",
                        column: x => x.EpicId,
                        principalTable: "Issues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Issues_EpicId",
                table: "Issues",
                column: "EpicId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_IssueNumber",
                table: "Issues",
                column: "IssueNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_Project",
                table: "Issues",
                column: "Project");

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
