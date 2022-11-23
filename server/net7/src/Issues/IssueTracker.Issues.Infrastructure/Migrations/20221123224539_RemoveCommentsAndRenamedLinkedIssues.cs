using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.Issues.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCommentsAndRenamedLinkedIssues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueLinks_Issues_RightId",
                table: "IssueLinks");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.RenameColumn(
                name: "Child",
                table: "IssueLinks",
                newName: "LinkType");

            migrationBuilder.RenameColumn(
                name: "RightId",
                table: "IssueLinks",
                newName: "ChildId");

            migrationBuilder.RenameIndex(
                name: "IX_IssueLinks_RightId",
                table: "IssueLinks",
                newName: "IX_IssueLinks_ChildId");

            migrationBuilder.AlterColumn<Guid>(
                name: "Reporter_UserId",
                table: "Issues",
                type: "TEXT",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldDefaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Reporter_FullName",
                table: "Issues",
                type: "TEXT",
                maxLength: 200,
                nullable: true,
                defaultValue: "Unassigned",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200,
                oldDefaultValue: "Unassigned");

            migrationBuilder.AlterColumn<Guid>(
                name: "Assignee_UserId",
                table: "Issues",
                type: "TEXT",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldDefaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Assignee_FullName",
                table: "Issues",
                type: "TEXT",
                maxLength: 200,
                nullable: true,
                defaultValue: "Unassigned",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200,
                oldDefaultValue: "Unassigned");

            migrationBuilder.AddForeignKey(
                name: "FK_IssueLinks_Issues_ChildId",
                table: "IssueLinks",
                column: "ChildId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueLinks_Issues_ChildId",
                table: "IssueLinks");

            migrationBuilder.RenameColumn(
                name: "LinkType",
                table: "IssueLinks",
                newName: "Child");

            migrationBuilder.RenameColumn(
                name: "ChildId",
                table: "IssueLinks",
                newName: "RightId");

            migrationBuilder.RenameIndex(
                name: "IX_IssueLinks_ChildId",
                table: "IssueLinks",
                newName: "IX_IssueLinks_RightId");

            migrationBuilder.AlterColumn<Guid>(
                name: "Reporter_UserId",
                table: "Issues",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Reporter_FullName",
                table: "Issues",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "Unassigned",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200,
                oldNullable: true,
                oldDefaultValue: "Unassigned");

            migrationBuilder.AlterColumn<Guid>(
                name: "Assignee_UserId",
                table: "Issues",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Assignee_FullName",
                table: "Issues",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "Unassigned",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200,
                oldNullable: true,
                oldDefaultValue: "Unassigned");

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AuthorFullName = table.Column<string>(name: "Author_FullName", type: "TEXT", maxLength: 200, nullable: false),
                    AuthorUserId = table.Column<Guid>(name: "Author_UserId", type: "TEXT", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<long>(type: "INTEGER", nullable: false),
                    IssueId = table.Column<string>(type: "TEXT", nullable: false),
                    LastModifiedTime = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_IssueLinks_Issues_RightId",
                table: "IssueLinks",
                column: "RightId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
