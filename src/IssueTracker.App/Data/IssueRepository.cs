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

using System.Runtime.CompilerServices;
using IssueTracker.App.Model;
using IssueTracker.App.Model.Projections;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.App.Data;

public sealed class IssueRepository
{
    private readonly ApplicationDbContext _dbContext;

    public IssueRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbContext.Database.Migrate();
    }

    public async IAsyncEnumerable<IssueSummaryProjection> GetIssueSummaries([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IAsyncEnumerable<IssueSummaryProjection> issues = _dbContext.Issues
            .AsNoTracking()
            .Select(i => new IssueSummaryProjection(i.Id, i.Name))
            .AsAsyncEnumerable();
        await foreach (IssueSummaryProjection issue in issues.WithCancellation(cancellationToken))
        {
            yield return issue;
        }
    }

    public Task<Issue?> GetIssueById(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Issues
            .AsNoTracking()
            .SingleOrDefaultAsync(i => i.Id == id, cancellationToken);
    }
}
