using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.Issues.Infrastructure.Migrations
{
    public partial class StartStopTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "_startTime",
                table: "Issues",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "_stopTime",
                table: "Issues",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_startTime",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "_stopTime",
                table: "Issues");
        }
    }
}
