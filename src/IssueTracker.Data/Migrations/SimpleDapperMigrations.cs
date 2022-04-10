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

using System.Collections.Generic;
using System.Data;
using System.Linq;
using IssueTracker.Data.Migrations.SimpleDapper;
using IssueTracker.Data.Model;
using Dapper;

namespace IssueTracker.Data.Migrations;

public sealed class SimpleDapperMigrations 
{
    public void Migrate(IDbConnection connection)
    {
        List<string> existingMigrations = GetMigraitons(connection).ToList();
        List<IMigration> migrations = Migrations.ToList();


        IEnumerable<IMigration> toRemove = existingMigrations
            .Select(id => migrations.FirstOrDefault(m => m.Id == id))
            .Where(migration => migration is not null);

        foreach (IMigration migration in toRemove)
        {
            migrations.Remove(migration);
        }

        migrations.ForEach(m => m.Up(connection));
    }

    private static IEnumerable<string> GetMigraitons(IDbConnection connection)
    {
        return connection
            .Query<Migration>(@"SELECT ""MigrationId"" FROM Migrations")
            .Select(m => m.MigrationId);
    }

    private IEnumerable<IMigration> Migrations
    {
        get
        {
            yield return new InitialMigration20220410();
        }
    }
}
