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

using GraphQL.Types;
using IssueTracker.Issues.API.GraphQL.DataTransferObjects;
using IssueTracker.Issues.API.GraphQL.Types;
using IssueTracker.Issues.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IssueTracker.Issues.API.GraphQL.Queries;

/// <summary/>
public sealed class IssueQuery : ObjectGraphType<object>
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="IssueQuery"/> class.
    /// </summary>
    public IssueQuery()
    {
        Name = "IssueQuery";
        FieldAsync<ListGraphType<IssueModelType>>("issues",
            resolve: context => ResolveIssues(
                context.RequestServices?.GetService<IssuesDbContext>(),
                context.CancellationToken));
    }

    private static Task<object?> ResolveIssues(IssuesDbContext? dbContext,
        CancellationToken cancellationToken)
    {
        if (dbContext is null)
        {
            return Task.FromResult<object?>(new List<IssueDto>());
        }

        return dbContext.Issues
            .AsNoTracking()
            .Include("_relatedTo")
            .Include("_relatedFrom")
            .Select(i => new
            {
                i.Id,
                i.Title,
                i.Description,
                i.Priority,
                i.Type,
                i.State,
                i.Assignee,
                i.Reporter,
                i.EpicId
            })
            .AsAsyncEnumerable()
            .Select(i => new IssueDto(
                i.Id.ToString(),
                i.Title,
                i.Description,
                i.Priority,
                i.Type,
                i.State.Value,
                new TriageUserDto(i.Reporter.UserId, i.Reporter.FullName),
                new MaintainerDto(i.Reporter.UserId, i.Reporter.FullName),
                i.EpicId?.ToString()))
            .ToArrayAsync(cancellationToken)
            .AsTask()
            .ContinueWith(
                t => (object?)t.Result,
                TaskContinuationOptions.OnlyOnRanToCompletion);
    }
}
