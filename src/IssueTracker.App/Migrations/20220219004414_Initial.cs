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
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
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
                columns: new[] { "Id", "ConcurrencyToken", "Description", "LastUpdated", "Name", "Priority" },
                values: new object[] { new Guid("5d207bf8-2005-4a15-9dc4-d627c3d5e5e6"), "b664f100-0834-454d-b430-ee3675272431", "Second issue", new DateTime(2022, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Second", 0 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "ConcurrencyToken", "Description", "LastUpdated", "Name", "Priority" },
                values: new object[] { new Guid("e6156d06-703b-4227-b2ea-ad3bb9fc6350"), "d9f44016-e349-4416-a981-99a930e41bf5", "First issue", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "First", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Issues_Name",
                table: "Issues",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_Priority",
                table: "Issues",
                column: "Priority");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Issues");
        }
    }
}
