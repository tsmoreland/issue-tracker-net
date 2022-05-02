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

using AutoMapper;
using GraphQL.Types;
using IssueTracker.Core.ValueObjects;
using IssueTracker.Core.Views;
using IssueTracker.Data.Abstractions;
using IssueTracker.GraphQl.App.Model.Response;

namespace IssueTracker.GraphQl.App.Services.Types;

/// <summary>
/// Issue Type
/// </summary>
public sealed class IssueModelType : ObjectGraphType<IssueDto>
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="IssueModelType"/> class.
    /// </summary>
    public IssueModelType()
    {
        Field(f => f.IssueId, nullable: false).Description("Id");
        Field(f => f.Title, nullable: false).Description("Title");
        Field(f => f.Description).Description("Description");
        Field(f => f.Priority, nullable: false).Description("Priority");
        Field(f => f.Type, nullable: false).Description("Issue Type");
        FieldAsync<ListGraphType<LinkedIssueType>>("parents",
            resolve: context =>
                ResolveIssueParents(
                    context.Source.IssueId.ToString(),
                    context.RequestServices?.GetService<IIssueRepository>(),
                    context.RequestServices?.GetService<IMapper>(),
                    context.CancellationToken));
        FieldAsync<ListGraphType<LinkedIssueType>>("children",
            resolve: context =>
                ResolveIssueChildren(
                    context.Source.IssueId.ToString(),
                    context.RequestServices?.GetService<IIssueRepository>(),
                    context.RequestServices?.GetService<IMapper>(),
                    context.CancellationToken));
        Field(f => f.Assignee, nullable: false).Description("Assigned user");
        Field(f => f.Reporter, nullable: false).Description("Assigned user");
    }

    internal static async Task<object?> ResolveIssueParents(string id, IIssueRepository? repository, IMapper? mapper,
        CancellationToken cancellationToken)
    {
        return repository is not null && mapper is not null
            ? await repository.GetParentIssues(IssueIdentifier.FromString(id), cancellationToken)
                .Select(mapper.Map<LinkedIssueViewDto>)
                .ToHashSetAsync(cancellationToken)
            : new HashSet<LinkedIssueView>();
    }

    internal static async Task<object?> ResolveIssueChildren(string id, IIssueRepository? repository, IMapper? mapper,
        CancellationToken cancellationToken)
    {
        return repository is not null && mapper is not null
            ? await repository.GetChildIssues(IssueIdentifier.FromString(id), cancellationToken)
                .Select(mapper.Map<LinkedIssueViewDto>)
                .ToHashSetAsync(cancellationToken)
            : new HashSet<LinkedIssueView>();
    }
}
