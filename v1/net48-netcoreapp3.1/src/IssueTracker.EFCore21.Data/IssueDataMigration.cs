// 
// Copyright (c) 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.EFCore21.Data;

public sealed class IssueDataMigration : IIssueDataMigration
{
    private readonly IssuesDbContext _dbContext;

    public IssueDataMigration(IssuesDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <inheritdoc />
    public Task MigrateAsync(CancellationToken cancellationToken)
    {
        return _dbContext.Database.MigrateAsync(cancellationToken);
    }

    /// <inheritdoc />
    public void Migrate()
    {
        _dbContext.Database.Migrate();
    }

    /// <inheritdoc />
    public Task SeedAync(IIssueRepository repository, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
