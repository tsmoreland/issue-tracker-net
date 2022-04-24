using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.Data.Migrations
{
    public partial class UpdateSeededData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("1385056e-8afa-4e09-96df-ae12efdf1a29"),
                column: "ConcurrencyToken",
                value: "bcc736d3-3bfe-4d17-b78b-28f6607609b9");

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("502ad68e-7b37-4426-b422-23b6a9b1b7ca"),
                column: "ConcurrencyToken",
                value: "2727e28c-bd95-4366-8a96-821fcc63ef9d");

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("a28b8c45-6668-4169-940c-c16d71eb69de"),
                column: "ConcurrencyToken",
                value: "25b0b4c8-101c-445b-8242-05b61759ba00");

            migrationBuilder.UpdateData(
                table: "LinkedIssue",
                keyColumns: new[] { "ChildIssueId", "ParentIssueId" },
                keyValues: new object[] { new Guid("502ad68e-7b37-4426-b422-23b6a9b1b7ca"), new Guid("a28b8c45-6668-4169-940c-c16d71eb69de") },
                columns: new[] { "ConcurrencyToken", "Id" },
                values: new object[] { "8b72b6ef-d05b-4a5b-8ee2-91821c08d0e0", new Guid("2bb51033-476e-4ca0-bfd7-d1aaeafd9b1c") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
