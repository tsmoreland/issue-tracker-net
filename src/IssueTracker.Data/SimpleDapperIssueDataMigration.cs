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
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Data.Abstractions;
using Microsoft.Data.Sqlite;
using Dapper;
using IssueTracker.Data.Migrations;

namespace IssueTracker.Data;

public sealed class SimpleDapperIssueDataMigration : IIssueDataMigration
{
    private readonly DatabaseOptions _databaseOptions;

    public SimpleDapperIssueDataMigration(DatabaseOptions databaseOptions)
    {
        _databaseOptions = databaseOptions ?? throw new ArgumentNullException(nameof(databaseOptions));
    }

    /// <inheritdoc />
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        SQLitePCL.Batteries.Init();

        if (!_databaseOptions.DatabaseExists())
        {
            System.IO.File.WriteAllBytes(_databaseOptions.DatabaseFileName, Array.Empty<byte>());
        }

        IDbConnection connection = new SqliteConnection(_databaseOptions.ConnectionString);
        try
        {
            connection.Open();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return;
        }

        await connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS ""Migrations""
(
  ""MigrationId"" TEXT NOT NULL CONSTRAINT ""PK_MigrationId"" PRIMARY KEY
)");

        SimpleDapperMigrations migrations = new ();
        migrations.Migrate(connection);
    }

    /// <inheritdoc />
    public void Migrate()
    {
        // generally not recommended but keeping this simple for now
        MigrateAsync(CancellationToken.None).Wait();
    }
}
