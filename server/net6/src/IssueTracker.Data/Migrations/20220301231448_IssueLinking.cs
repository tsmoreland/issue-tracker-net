using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.Data.Migrations
{
    public partial class IssueLinking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LinkedIssue",
                columns: table => new
                {
                    ParentIssueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChildIssueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LinkType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkedIssue", x => new { x.ParentIssueId, x.ChildIssueId });
                    table.ForeignKey(
                        name: "FK_LinkedIssue_Issues_ChildIssueId",
                        column: x => x.ChildIssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LinkedIssue_Issues_ParentIssueId",
                        column: x => x.ParentIssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LinkedIssue_ChildIssueId",
                table: "LinkedIssue",
                column: "ChildIssueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkedIssue");
        }
    }
}
