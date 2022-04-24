using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IssueTracker.EFCore21.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FullName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    AssigneeId = table.Column<Guid>(nullable: true),
                    ReporterId = table.Column<Guid>(nullable: true),
                    LastUpdated = table.Column<long>(nullable: false),
                    ConcurrencyToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Issues_User_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Issues_User_ReporterId",
                        column: x => x.ReporterId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LinkedIssue",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LinkType = table.Column<int>(nullable: false),
                    ParentIssueId = table.Column<int>(nullable: false),
                    ChildIssueId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkedIssue", x => new { x.ParentIssueId, x.ChildIssueId });
                    table.UniqueConstraint("AK_LinkedIssue_Id", x => x.Id);
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

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "AssigneeId", "ConcurrencyToken", "Description", "LastUpdated", "Priority", "ReporterId", "Title", "Type" },
                values: new object[] { 1, null, "4f0c7ea0-6e38-4610-9224-cfba1056ee07", "First issue", 637765920000000000L, 1, null, "First", 0 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "AssigneeId", "ConcurrencyToken", "Description", "LastUpdated", "Priority", "ReporterId", "Title", "Type" },
                values: new object[] { 2, null, "57935055-4216-4132-b2f5-f146eba5d613", "Second issue", 637782336000000000L, 0, null, "Second", 1 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "AssigneeId", "ConcurrencyToken", "Description", "LastUpdated", "Priority", "ReporterId", "Title", "Type" },
                values: new object[] { 3, null, "bb29910b-f0e4-4958-98d1-cea4572a3b82", "Third issue", 637782336000000000L, 1, null, "Third", 1 });

            migrationBuilder.InsertData(
                table: "LinkedIssue",
                columns: new[] { "ParentIssueId", "ChildIssueId", "Id", "LinkType" },
                values: new object[] { 2, 3, 1, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Issues_AssigneeId",
                table: "Issues",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_Priority",
                table: "Issues",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_ReporterId",
                table: "Issues",
                column: "ReporterId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_Title",
                table: "Issues",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_LinkedIssue_ChildIssueId",
                table: "LinkedIssue",
                column: "ChildIssueId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkedIssue_ParentIssueId",
                table: "LinkedIssue",
                column: "ParentIssueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkedIssue");

            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
