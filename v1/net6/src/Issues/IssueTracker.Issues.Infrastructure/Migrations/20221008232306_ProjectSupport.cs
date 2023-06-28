using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.Issues.Infrastructure.Migrations
{
    public partial class ProjectSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Project",
                table: "Issues",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Issues_Project",
                table: "Issues",
                newName: "IX_Issues_ProjectId");

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    LastModifiedTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Id",
                table: "Projects",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name",
                table: "Projects",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Projects_ProjectId",
                table: "Issues",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Projects_ProjectId",
                table: "Issues");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Issues",
                newName: "Project");

            migrationBuilder.RenameIndex(
                name: "IX_Issues_ProjectId",
                table: "Issues",
                newName: "IX_Issues_Project");
        }
    }
}
