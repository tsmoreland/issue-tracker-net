//
// Copyright © 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using IssueTracker.Core.Model;

namespace IssueTracker.Data.Migrations.SimpleDapper;

internal sealed class InitialMigration20220410 : MigrationBase
{
    /// <inheritdoc />
    public override string Id => "20220410_Initial";

    /// <inheritdoc />
    public override void Up(IDbConnection connection)
    {
        using IDbTransaction transaction = connection.BeginTransaction();

        connection.Execute(@"CREATE TABLE IF NOT EXISTS ""Issues""
(
  ""Id"" TEXT NOT NULL CONSTRAINT ""PK_Issues"" PRIMARY KEY,
  ""Title"" TEXT NOT NULL,
  ""Description"" TEXT NOT NULL,
  ""Priority"" INTEGER NOT NULL,
  ""LastUpdated"" TEXT NOT NULL,
  ""ConcurrencyToken"" TEXT NULL,
  ""Type"" INTEGER NOT NULL DEFAULT 0
)");

        connection.Execute(@"CREATE TABLE IF NOT EXISTS ""LinkedIssue""
(
  ""ParentIssueId"" TEXT NOT NULL,
  ""ChildIssueId"" TEXT NOT NULL,
  ""LinkType"" INTEGER NOT NULL,
  CONSTRAINT ""PK_LinkedIssue"" PRIMARY KEY (""ParentIssueId"", ""ChildIssueId""),
  CONSTRAINT ""FK_LinkedIssue_Issues_ChildIssueId"" FOREIGN KEY (""ChildIssueId"") REFERENCES ""Issues"" (""Id"") ON DELETE RESTRICT,
  CONSTRAINT ""FK_LinkedIssue_Issues_ParentIssueId"" FOREIGN KEY (""ParentIssueId"") REFERENCES ""Issues"" (""Id"") ON DELETE RESTRICT
)");


        foreach (Issue issue in SeedIssues())
        {
            connection.Execute(@"INSERT INTO Issues
(""Id"", ""Title"", ""Description"", ""Priority"", ""LastUpdated"", ""ConcurrencyToken"", ""Type"")
VALUES
(@Id, @Title, @Description, @Priority, @LastUpdated, @ConcurrencyToken, @Type)", issue);
        }

        AddMigrationId(connection);

        transaction.Commit();
    }

    /// <inheritdoc />
    public override void Down(IDbConnection connection)
    {
    }

    private static IEnumerable<Issue> SeedIssues()
    {
        yield return new Issue(new Guid("1385056E-8AFA-4E09-96DF-AE12EFDF1A29"), "First", "First issue",
            Priority.Medium, IssueType.Epic,
            new DateTime(2022, 01, 01, 0, 0, 0, DateTimeKind.Utc), Guid.NewGuid().ToString(),
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>());
        yield return new Issue(new Guid("A28B8C45-6668-4169-940C-C16D71EB69DE"), "Second", "Second issue",
            Priority.Low, IssueType.Story,
            new DateTime(2022, 01, 020, 0, 0, 0, DateTimeKind.Utc), Guid.NewGuid().ToString(),
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>());
        yield return new Issue(new Guid("502AD68E-7B37-4426-B422-23B6A9B1B7CA"), "Third", "Third issue",
            Priority.Medium, IssueType.Story,
            new DateTime(2022, 01, 020, 0, 0, 0, DateTimeKind.Utc), Guid.NewGuid().ToString(),
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>());
    }
}
