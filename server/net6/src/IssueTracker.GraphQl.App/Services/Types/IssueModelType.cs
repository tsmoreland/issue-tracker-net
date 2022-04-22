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

using GraphQL.Types;
using IssueTracker.Core.Model;
using IssueTracker.Core.Views;
using IssueTracker.Data.Abstractions;

namespace IssueTracker.GraphQl.App.Services.Types;

/// <summary>
/// Issue Type
/// </summary>
public sealed class IssueModelType : ObjectGraphType<Issue>
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="IssueModelType"/> class.
    /// </summary>
    public IssueModelType()
    {
        Field(f => f.Id, nullable: false).Description("Identifier");
        Field(f => f.Title, nullable: false).Description("Title");
        Field(f => f.Description).Description("Description");
        Field(f => f.Priority, nullable: false).Description("Priority");
        Field(f => f.Type, nullable: false).Description("Issue Type");
        FieldAsync<ListGraphType<LinkedIssueType>>("parents",
            resolve: context =>
                ResolveIssueParents(
                    context.Source.Id,
                    context.RequestServices?.GetService<IIssueRepository>(),
                    context.CancellationToken));
        FieldAsync<ListGraphType<LinkedIssueType>>("children",
            resolve: context =>
                ResolveIssueChildren(
                    context.Source.Id,
                    context.RequestServices?.GetService<IIssueRepository>(),
                    context.CancellationToken));
    }

    internal static async Task<object?> ResolveIssueParents(Guid id, IIssueRepository? repository,
        CancellationToken cancellationToken)
    {
        return repository is not null
            ? await repository.GetParentIssues(id, cancellationToken).ToListAsync(cancellationToken)
            : new List<LinkedIssueView>();
    }

    internal static async Task<object?> ResolveIssueChildren(Guid id, IIssueRepository? repository,
        CancellationToken cancellationToken)
    {
        return repository is not null
            ? await repository.GetChildIssues(id, cancellationToken).ToListAsync(cancellationToken)
            : new List<LinkedIssueView>();
    }
}
