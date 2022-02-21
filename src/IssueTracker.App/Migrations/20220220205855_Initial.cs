using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.App.Migrations
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
                values: new object[] { new Guid("1385056e-8afa-4e09-96df-ae12efdf1a29"), "c431b935-f23e-4d82-8473-3f9c8514a5e3", "First issue", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "First" });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "ConcurrencyToken", "Description", "LastUpdated", "Priority", "Title" },
                values: new object[] { new Guid("a28b8c45-6668-4169-940c-c16d71eb69de"), "f1c76a08-04bd-44ee-93bd-52e09297f004", "Second issue", new DateTime(2022, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Second" });

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
