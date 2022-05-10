using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.Data.Migrations
{
    public partial class AddEpicIssueId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: new Guid("fb648532-026f-447c-a9c7-996fac6a8b2e"));

            migrationBuilder.DeleteData(
                table: "LinkedIssues",
                keyColumns: new[] { "ChildIssueId", "ParentIssueId" },
                keyValues: new object[] { new Guid("1642a18b-96a1-46f0-a834-5fcb72e14aaa"), new Guid("9776c88f-d2cc-4e01-8d34-56710cbd2be4") });

            migrationBuilder.DeleteData(
                table: "LinkedIssues",
                keyColumns: new[] { "ChildIssueId", "ParentIssueId" },
                keyValues: new object[] { new Guid("b3941d54-f1f9-4b90-9e84-c7f33cee6d97"), new Guid("ef0b233b-bed3-4027-9ed9-010fe00ecf2f") });

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: new Guid("1642a18b-96a1-46f0-a834-5fcb72e14aaa"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: new Guid("9776c88f-d2cc-4e01-8d34-56710cbd2be4"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: new Guid("b3941d54-f1f9-4b90-9e84-c7f33cee6d97"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: new Guid("ef0b233b-bed3-4027-9ed9-010fe00ecf2f"));

            migrationBuilder.AddColumn<Guid>(
                name: "EpicIssueId",
                table: "Issues",
                type: "TEXT",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "EpicIssueId", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("33e3d8bf-af55-498a-b96f-f514a3b0a39d"), "c5cbc3aa-0a9e-4ccb-96de-b4bf61854d5b", "", null, "APP-2", 2, 637766376000000000L, 1, "APP", "The database should story issues with links to projects", 1 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "EpicIssueId", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("794c169c-a1dd-45ee-b093-fd80c25a28bf"), "132e3b89-ce8d-4ed7-9a6b-81410c11832e", "add the model(s) for project type ensuring it's id matches the expectations of issue", null, "APP-4", 4, 637767216000000000L, 1, "APP", "add project core models", 2 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "EpicIssueId", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("b0454fc6-9014-4dc8-bccc-a90b997e7b02"), "4a9f3a8b-5618-4daf-b6db-a254857f5287", "add the request handlers to get projects by id and a summary", null, "APP-5", 5, 637768143000000000L, 0, "APP", "add mediator request/handlers for project query", 2 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "EpicIssueId", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("cd9a0df2-5f83-4942-9c08-4d97b2a90d55"), "1c0b0db4-1a94-4af7-903e-9bbf8fc71ce6", "As a user I want to be able to retreive all projects", null, "APP-3", 3, 637766379000000000L, 0, "APP", "The api should be able to retrieve projects", 1 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "EpicIssueId", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("ee88fef9-ccf4-41b0-8326-1200f60b308b"), "a8bd5368-4ce4-4d60-98a5-e89e0c15d855", "add projects and use project id as link between issue and project", null, "APP-1", 1, 637766370000000000L, 2, "APP", "Add Project support", 0 });

            migrationBuilder.InsertData(
                table: "LinkedIssues",
                columns: new[] { "ChildIssueId", "ParentIssueId", "ChildId", "ConcurrencyToken", "LinkType", "ParentId" },
                values: new object[] { new Guid("cd9a0df2-5f83-4942-9c08-4d97b2a90d55"), new Guid("33e3d8bf-af55-498a-b96f-f514a3b0a39d"), "APP-3", "6082d1e3-d7a5-413d-873e-9bca4715d346", 0, "APP-2" });

            migrationBuilder.InsertData(
                table: "LinkedIssues",
                columns: new[] { "ChildIssueId", "ParentIssueId", "ChildId", "ConcurrencyToken", "LinkType", "ParentId" },
                values: new object[] { new Guid("b0454fc6-9014-4dc8-bccc-a90b997e7b02"), new Guid("794c169c-a1dd-45ee-b093-fd80c25a28bf"), "APP-5", "69925489-fdbf-4645-a0a7-928316b6d3b7", 2, "APP-4" });

            migrationBuilder.CreateIndex(
                name: "IX_Issues_EpicIssueId",
                table: "Issues",
                column: "EpicIssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Issues_EpicIssueId",
                table: "Issues",
                column: "EpicIssueId",
                principalTable: "Issues",
                principalColumn: "IssueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Issues_EpicIssueId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_EpicIssueId",
                table: "Issues");

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: new Guid("ee88fef9-ccf4-41b0-8326-1200f60b308b"));

            migrationBuilder.DeleteData(
                table: "LinkedIssues",
                keyColumns: new[] { "ChildIssueId", "ParentIssueId" },
                keyValues: new object[] { new Guid("cd9a0df2-5f83-4942-9c08-4d97b2a90d55"), new Guid("33e3d8bf-af55-498a-b96f-f514a3b0a39d") });

            migrationBuilder.DeleteData(
                table: "LinkedIssues",
                keyColumns: new[] { "ChildIssueId", "ParentIssueId" },
                keyValues: new object[] { new Guid("b0454fc6-9014-4dc8-bccc-a90b997e7b02"), new Guid("794c169c-a1dd-45ee-b093-fd80c25a28bf") });

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: new Guid("33e3d8bf-af55-498a-b96f-f514a3b0a39d"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: new Guid("794c169c-a1dd-45ee-b093-fd80c25a28bf"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: new Guid("b0454fc6-9014-4dc8-bccc-a90b997e7b02"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: new Guid("cd9a0df2-5f83-4942-9c08-4d97b2a90d55"));

            migrationBuilder.DropColumn(
                name: "EpicIssueId",
                table: "Issues");

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("1642a18b-96a1-46f0-a834-5fcb72e14aaa"), "e86ac0ac-80ba-4108-8aa3-7265c8240a68", "As a user I want to be able to retreive all projects", "APP-3", 3, 637766379000000000L, 0, "APP", "The api should be able to retrieve projects", 1 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("9776c88f-d2cc-4e01-8d34-56710cbd2be4"), "4446f14e-71e1-415a-aab4-78bdae749755", "", "APP-2", 2, 637766376000000000L, 1, "APP", "The database should story issues with links to projects", 1 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("b3941d54-f1f9-4b90-9e84-c7f33cee6d97"), "b32115cc-d27b-4aeb-b9fc-5a50f6b4ae33", "add the request handlers to get projects by id and a summary", "APP-5", 5, 637768143000000000L, 0, "APP", "add mediator request/handlers for project query", 2 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("ef0b233b-bed3-4027-9ed9-010fe00ecf2f"), "474effa9-b4a4-4a0b-ad02-2d2c5490aad1", "add the model(s) for project type ensuring it's id matches the expectations of issue", "APP-4", 4, 637767216000000000L, 1, "APP", "add project core models", 2 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("fb648532-026f-447c-a9c7-996fac6a8b2e"), "bad845f6-cdb9-4148-b24c-051d783796ff", "add projects and use project id as link between issue and project", "APP-1", 1, 637766370000000000L, 2, "APP", "Add Project support", 0 });

            migrationBuilder.InsertData(
                table: "LinkedIssues",
                columns: new[] { "ChildIssueId", "ParentIssueId", "ChildId", "ConcurrencyToken", "LinkType", "ParentId" },
                values: new object[] { new Guid("1642a18b-96a1-46f0-a834-5fcb72e14aaa"), new Guid("9776c88f-d2cc-4e01-8d34-56710cbd2be4"), "APP-3", "95904269-574f-4135-9ed2-3abd801e15e9", 0, "APP-2" });

            migrationBuilder.InsertData(
                table: "LinkedIssues",
                columns: new[] { "ChildIssueId", "ParentIssueId", "ChildId", "ConcurrencyToken", "LinkType", "ParentId" },
                values: new object[] { new Guid("b3941d54-f1f9-4b90-9e84-c7f33cee6d97"), new Guid("ef0b233b-bed3-4027-9ed9-010fe00ecf2f"), "APP-5", "198375d6-fc95-4d80-92e1-96f7c1438d58", 2, "APP-4" });
        }
    }
}
