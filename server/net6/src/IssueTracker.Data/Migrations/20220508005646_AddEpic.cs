using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.Data.Migrations
{
    public partial class AddEpic : Migration
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
                values: new object[] { new Guid("06c3e187-0486-4eed-91db-b346a7044879"), "b2d2aac4-77e6-415a-89b9-711d37f13636", "add projects and use project id as link between issue and project", null, "APP-1", 1, 637766370000000000L, 2, "APP", "Add Project support", 0 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "EpicIssueId", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("47cc7642-a8ed-4cc7-ba94-3c2ed8fc2165"), "3d9f247a-82a1-43d4-8928-76a94c245a8d", "", null, "APP-2", 2, 637766376000000000L, 1, "APP", "The database should story issues with links to projects", 1 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "EpicIssueId", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("cda71c85-f63b-42eb-8f48-f0ad1409e10d"), "3ced4422-84d5-4d7d-8701-b56d660ac6c1", "As a user I want to be able to retreive all projects", null, "APP-3", 3, 637766379000000000L, 0, "APP", "The api should be able to retrieve projects", 1 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "EpicIssueId", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("d180b554-0a2b-4e44-bb57-82075f4f5fbf"), "254447bd-1ce7-4726-bc95-fe2330cb4556", "add the request handlers to get projects by id and a summary", null, "APP-5", 5, 637768143000000000L, 0, "APP", "add mediator request/handlers for project query", 2 });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "ConcurrencyToken", "Description", "EpicIssueId", "Id", "IssueNumber", "LastUpdated", "Priority", "Project", "Title", "Type" },
                values: new object[] { new Guid("ddbb6026-129b-474e-af0a-4f58a9dd68b4"), "45217bdc-4936-445d-b6ef-703eab053412", "add the model(s) for project type ensuring it's id matches the expectations of issue", null, "APP-4", 4, 637767216000000000L, 1, "APP", "add project core models", 2 });

            migrationBuilder.InsertData(
                table: "LinkedIssues",
                columns: new[] { "ChildIssueId", "ParentIssueId", "ChildId", "ConcurrencyToken", "LinkType", "ParentId" },
                values: new object[] { new Guid("cda71c85-f63b-42eb-8f48-f0ad1409e10d"), new Guid("47cc7642-a8ed-4cc7-ba94-3c2ed8fc2165"), "APP-3", "4962858d-e5a8-4143-872f-bd19ba370d9c", 0, "APP-2" });

            migrationBuilder.InsertData(
                table: "LinkedIssues",
                columns: new[] { "ChildIssueId", "ParentIssueId", "ChildId", "ConcurrencyToken", "LinkType", "ParentId" },
                values: new object[] { new Guid("d180b554-0a2b-4e44-bb57-82075f4f5fbf"), new Guid("ddbb6026-129b-474e-af0a-4f58a9dd68b4"), "APP-5", "1c386db3-4549-498c-a55b-49231a9d0ddf", 2, "APP-4" });

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
                keyValue: new Guid("06c3e187-0486-4eed-91db-b346a7044879"));

            migrationBuilder.DeleteData(
                table: "LinkedIssues",
                keyColumns: new[] { "ChildIssueId", "ParentIssueId" },
                keyValues: new object[] { new Guid("cda71c85-f63b-42eb-8f48-f0ad1409e10d"), new Guid("47cc7642-a8ed-4cc7-ba94-3c2ed8fc2165") });

            migrationBuilder.DeleteData(
                table: "LinkedIssues",
                keyColumns: new[] { "ChildIssueId", "ParentIssueId" },
                keyValues: new object[] { new Guid("d180b554-0a2b-4e44-bb57-82075f4f5fbf"), new Guid("ddbb6026-129b-474e-af0a-4f58a9dd68b4") });

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: new Guid("47cc7642-a8ed-4cc7-ba94-3c2ed8fc2165"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: new Guid("cda71c85-f63b-42eb-8f48-f0ad1409e10d"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: new Guid("d180b554-0a2b-4e44-bb57-82075f4f5fbf"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: new Guid("ddbb6026-129b-474e-af0a-4f58a9dd68b4"));

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
