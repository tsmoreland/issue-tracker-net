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
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    Reporter_UserId = table.Column<Guid>(type: "TEXT", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    Reporter_FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false, defaultValue: "Unassigned"),
                    Assignee_UserId = table.Column<Guid>(type: "TEXT", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    Assignee_FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false, defaultValue: "Unassigned"),
                    ConcurrencyToken = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    EpicId = table.Column<string>(type: "TEXT", nullable: true),
                    IssueNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Project = table.Column<string>(type: "TEXT", unicode: false, maxLength: 3, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "IssueLinks",
                columns: table => new
                {
                    LeftId = table.Column<string>(type: "TEXT", nullable: false),
                    RightId = table.Column<string>(type: "TEXT", nullable: false),
                    Link = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueLinks", x => new { x.LeftId, x.RightId });
                    table.ForeignKey(
                        name: "FK_IssueLinks_Issues_LeftId",
                        column: x => x.LeftId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IssueLinks_Issues_RightId",
                        column: x => x.RightId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IssueLinks_RightId",
                table: "IssueLinks",
                column: "RightId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_EpicId",
                table: "Issues",
                column: "EpicId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssueLinks");

            migrationBuilder.DropTable(
                name: "Issues");
        }
    }
}
