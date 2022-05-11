using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.Issues.Infrastructure.Migrations
{
    public partial class RelatedIssueLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Link",
                table: "IssueLinks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "IssueLinks");
        }
    }
}
