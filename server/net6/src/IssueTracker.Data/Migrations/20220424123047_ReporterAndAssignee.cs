using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.Data.Migrations
{
    public partial class ReporterAndAssignee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkedIssue_Issues_ChildIssueId",
                table: "LinkedIssue");

            migrationBuilder.DropForeignKey(
                name: "FK_LinkedIssue_Issues_ParentIssueId",
                table: "LinkedIssue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LinkedIssue",
                table: "LinkedIssue");

            migrationBuilder.RenameTable(
                name: "LinkedIssue",
                newName: "LinkedIssues");

            migrationBuilder.RenameIndex(
                name: "IX_LinkedIssue_ChildIssueId",
                table: "LinkedIssues",
                newName: "IX_LinkedIssues_ChildIssueId");

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

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "LinkedIssues",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyToken",
                table: "LinkedIssues",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LinkedIssues",
                table: "LinkedIssues",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LinkedIssues_ParentIssueId",
                table: "LinkedIssues",
                column: "ParentIssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkedIssues_Issues_ChildIssueId",
                table: "LinkedIssues",
                column: "ChildIssueId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LinkedIssues_Issues_ParentIssueId",
                table: "LinkedIssues",
                column: "ParentIssueId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkedIssues_Issues_ChildIssueId",
                table: "LinkedIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_LinkedIssues_Issues_ParentIssueId",
                table: "LinkedIssues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LinkedIssues",
                table: "LinkedIssues");

            migrationBuilder.DropIndex(
                name: "IX_LinkedIssues_ParentIssueId",
                table: "LinkedIssues");

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

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LinkedIssues");

            migrationBuilder.DropColumn(
                name: "ConcurrencyToken",
                table: "LinkedIssues");

            migrationBuilder.RenameTable(
                name: "LinkedIssues",
                newName: "LinkedIssue");

            migrationBuilder.RenameIndex(
                name: "IX_LinkedIssues_ChildIssueId",
                table: "LinkedIssue",
                newName: "IX_LinkedIssue_ChildIssueId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LinkedIssue",
                table: "LinkedIssue",
                columns: new[] { "ParentIssueId", "ChildIssueId" });

            migrationBuilder.AddForeignKey(
                name: "FK_LinkedIssue_Issues_ChildIssueId",
                table: "LinkedIssue",
                column: "ChildIssueId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LinkedIssue_Issues_ParentIssueId",
                table: "LinkedIssue",
                column: "ParentIssueId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
