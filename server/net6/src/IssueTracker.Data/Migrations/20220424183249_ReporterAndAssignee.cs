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
                value: "53007193-8132-4573-bf1f-141c607b3321");

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("502ad68e-7b37-4426-b422-23b6a9b1b7ca"),
                column: "ConcurrencyToken",
                value: "99ba05ff-b97d-4854-a472-9d6bf33e3160");

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("a28b8c45-6668-4169-940c-c16d71eb69de"),
                column: "ConcurrencyToken",
                value: "f0c89753-83ef-4feb-8f2f-4529688ab9ba");

            migrationBuilder.UpdateData(
                table: "LinkedIssue",
                keyColumns: new[] { "ChildIssueId", "ParentIssueId" },
                keyValues: new object[] { new Guid("502ad68e-7b37-4426-b422-23b6a9b1b7ca"), new Guid("a28b8c45-6668-4169-940c-c16d71eb69de") },
                columns: new[] { "ConcurrencyToken", "Id" },
                values: new object[] { "f9de1f1c-84f9-46f3-9dad-e5372d40b829", new Guid("b5a14a5c-c894-4d1c-9c8c-66a93a4eece5") });

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
                column: "ConcurrencyToken",
                value: "c126a9bc-2ad5-4e33-a59e-e665c0c6dc35");

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("a28b8c45-6668-4169-940c-c16d71eb69de"),
                column: "ConcurrencyToken",
                value: "b9998e94-8a08-44e1-bc73-6e479fadee90");
        }
    }
}
