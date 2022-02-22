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
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "ConcurrencyToken", "Description", "LastUpdated", "Priority", "Title" },
                values: new object[] { new Guid("1385056e-8afa-4e09-96df-ae12efdf1a29"), "8ee1ef15-42b7-4dc2-843f-2bada4e449d7", "First issue", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "First" });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "ConcurrencyToken", "Description", "LastUpdated", "Priority", "Title" },
                values: new object[] { new Guid("a28b8c45-6668-4169-940c-c16d71eb69de"), "096122b5-3558-4430-b9e2-bff962596ae5", "Second issue", new DateTime(2022, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Second" });

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
