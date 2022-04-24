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

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("1385056e-8afa-4e09-96df-ae12efdf1a29"),
                column: "ConcurrencyToken",
                value: "d9b95aee-485b-4305-9d93-7f4489b0d81d");

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("a28b8c45-6668-4169-940c-c16d71eb69de"),
                columns: new[] { "ConcurrencyToken", "LastUpdated" },
                values: new object[] { "01b94dda-4412-45d3-af5a-d08532149d8a", new DateTime(2022, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "ConcurrencyToken", "Description", "LastUpdated", "Priority", "Title" },
                values: new object[] { new Guid("502ad68e-7b37-4426-b422-23b6a9b1b7ca"), "1384b03e-406b-40f4-abca-64070c5e4d91", "Third issue", new DateTime(2022, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Third" });

            migrationBuilder.InsertData(
                table: "LinkedIssue",
                columns: new[] { "ChildIssueId", "ParentIssueId", "LinkType" },
                values: new object[] { new Guid("502ad68e-7b37-4426-b422-23b6a9b1b7ca"), new Guid("a28b8c45-6668-4169-940c-c16d71eb69de"), 0 });

            migrationBuilder.CreateIndex(
                name: "IX_LinkedIssue_ChildIssueId",
                table: "LinkedIssue",
                column: "ChildIssueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkedIssue");

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("502ad68e-7b37-4426-b422-23b6a9b1b7ca"));

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("1385056e-8afa-4e09-96df-ae12efdf1a29"),
                column: "ConcurrencyToken",
                value: "8ee1ef15-42b7-4dc2-843f-2bada4e449d7");

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("a28b8c45-6668-4169-940c-c16d71eb69de"),
                columns: new[] { "ConcurrencyToken", "LastUpdated" },
                values: new object[] { "096122b5-3558-4430-b9e2-bff962596ae5", new DateTime(2022, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
