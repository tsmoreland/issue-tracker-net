using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.App.Migrations
{
    public partial class IssueConcurrencyToen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("5d207bf8-2005-4a15-9dc4-d627c3d5e5e6"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("e6156d06-703b-4227-b2ea-ad3bb9fc6350"));

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "ConcurrencyToken", "Description", "LastUpdated", "Name", "Priority" },
                values: new object[] { new Guid("7b4079ce-bead-4fa6-9c2e-6fb4b853615a"), "eb372a55-1209-449b-b499-d7c2894bd8cc", "Second issue", new DateTime(2022, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Second", 0 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "ConcurrencyToken", "Description", "LastUpdated", "Name", "Priority" },
                values: new object[] { new Guid("f8cd2eb4-6559-437f-a1f9-3adb7b886382"), "b7a843f5-6834-4d13-95b7-07fadc6ced87", "First issue", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "First", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("7b4079ce-bead-4fa6-9c2e-6fb4b853615a"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("f8cd2eb4-6559-437f-a1f9-3adb7b886382"));

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "ConcurrencyToken", "Description", "LastUpdated", "Name", "Priority" },
                values: new object[] { new Guid("5d207bf8-2005-4a15-9dc4-d627c3d5e5e6"), "b664f100-0834-454d-b430-ee3675272431", "Second issue", new DateTime(2022, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Second", 0 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "ConcurrencyToken", "Description", "LastUpdated", "Name", "Priority" },
                values: new object[] { new Guid("e6156d06-703b-4227-b2ea-ad3bb9fc6350"), "d9f44016-e349-4416-a981-99a930e41bf5", "First issue", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "First", 1 });
        }
    }
}
