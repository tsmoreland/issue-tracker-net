using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.Data.Migrations
{
    public partial class AddIssueType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Issues",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("1385056e-8afa-4e09-96df-ae12efdf1a29"),
                column: "ConcurrencyToken",
                value: "d9515a75-c02e-43d2-a47e-3e8be987924b");

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("502ad68e-7b37-4426-b422-23b6a9b1b7ca"),
                columns: new[] { "ConcurrencyToken", "Type" },
                values: new object[] { "c126a9bc-2ad5-4e33-a59e-e665c0c6dc35", 1 });

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("a28b8c45-6668-4169-940c-c16d71eb69de"),
                columns: new[] { "ConcurrencyToken", "Type" },
                values: new object[] { "b9998e94-8a08-44e1-bc73-6e479fadee90", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Issues");

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("1385056e-8afa-4e09-96df-ae12efdf1a29"),
                column: "ConcurrencyToken",
                value: "d9b95aee-485b-4305-9d93-7f4489b0d81d");

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("502ad68e-7b37-4426-b422-23b6a9b1b7ca"),
                column: "ConcurrencyToken",
                value: "1384b03e-406b-40f4-abca-64070c5e4d91");

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("a28b8c45-6668-4169-940c-c16d71eb69de"),
                column: "ConcurrencyToken",
                value: "01b94dda-4412-45d3-af5a-d08532149d8a");
        }
    }
}
