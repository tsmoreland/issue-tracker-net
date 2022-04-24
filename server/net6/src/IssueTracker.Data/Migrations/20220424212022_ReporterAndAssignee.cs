using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.Data.Migrations
{
    public partial class ReporterAndAssignee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyToken",
                table: "LinkedIssue",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "LinkedIssue",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<long>(
                name: "LastUpdated",
                table: "Issues",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "AssigneeFullName",
                table: "Issues",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "Unassigned");

            migrationBuilder.AddColumn<Guid>(
                name: "AssigneeId",
                table: "Issues",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ReporterFullName",
                table: "Issues",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "Unassigned");

            migrationBuilder.AddColumn<Guid>(
                name: "ReporterId",
                table: "Issues",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("1385056e-8afa-4e09-96df-ae12efdf1a29"),
                column: "ConcurrencyToken",
                value: "8f803cad-eb71-4912-95c3-ae73f63882ad");

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("502ad68e-7b37-4426-b422-23b6a9b1b7ca"),
                column: "ConcurrencyToken",
                value: "e020c8d1-20dd-451f-b560-a4b3347cd10a");

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("a28b8c45-6668-4169-940c-c16d71eb69de"),
                column: "ConcurrencyToken",
                value: "27b8bb20-b3f0-4998-a06f-da2aa874c643");

            migrationBuilder.UpdateData(
                table: "LinkedIssue",
                keyColumns: new[] { "ChildIssueId", "ParentIssueId" },
                keyValues: new object[] { new Guid("502ad68e-7b37-4426-b422-23b6a9b1b7ca"), new Guid("a28b8c45-6668-4169-940c-c16d71eb69de") },
                columns: new[] { "ConcurrencyToken", "Id" },
                values: new object[] { "f919c6a8-b038-4613-8a51-4f0522bc4c08", new Guid("270b0c6f-d622-495f-b85b-99238da73735") });

            migrationBuilder.CreateIndex(
                name: "IX_LinkedIssue_ParentIssueId",
                table: "LinkedIssue",
                column: "ParentIssueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LinkedIssue_ParentIssueId",
                table: "LinkedIssue");

            migrationBuilder.DropColumn(
                name: "ConcurrencyToken",
                table: "LinkedIssue");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LinkedIssue");

            migrationBuilder.DropColumn(
                name: "AssigneeFullName",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "AssigneeId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "ReporterFullName",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "ReporterId",
                table: "Issues");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdated",
                table: "Issues",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("1385056e-8afa-4e09-96df-ae12efdf1a29"),
                columns: new[] { "ConcurrencyToken", "LastUpdated" },
                values: new object[] { "d9515a75-c02e-43d2-a47e-3e8be987924b", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("502ad68e-7b37-4426-b422-23b6a9b1b7ca"),
                columns: new[] { "ConcurrencyToken", "LastUpdated" },
                values: new object[] { "c126a9bc-2ad5-4e33-a59e-e665c0c6dc35", new DateTime(2022, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("a28b8c45-6668-4169-940c-c16d71eb69de"),
                columns: new[] { "ConcurrencyToken", "LastUpdated" },
                values: new object[] { "b9998e94-8a08-44e1-bc73-6e479fadee90", new DateTime(2022, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc) });
        }
    }
}
