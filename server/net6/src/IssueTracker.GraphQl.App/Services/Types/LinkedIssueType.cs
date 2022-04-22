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
using IssueTracker.Core.Views;
using IssueTracker.Data.Abstractions;

namespace IssueTracker.GraphQl.App.Services.Types;

/// <summary>
/// Linked Issue Type
/// </summary>
public sealed class LinkedIssueType : ObjectGraphType<LinkedIssueView>
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="LinkedIssueType"/> class.
    /// </summary>
    public LinkedIssueType()
    {
        Field(f => f.Id, nullable: false).Description("Identifier");
        Field(f => f.Title, nullable: false).Description("Title");
        Field(f => f.Description).Description("Description");
        Field(f => f.Priority, nullable: false).Description("Priority");
        Field(f => f.Type, nullable: false).Description("Issue Type");
        Field(f => f.LinkType, nullable: false).Description("Issue Link Type");
        FieldAsync<ListGraphType<LinkedIssueType>>("parents",
            resolve: context =>
                IssueModelType.ResolveIssueParents(
                    context.Source.Id,
                    context.RequestServices?.GetService<IIssueRepository>(),
                    context.CancellationToken));
        FieldAsync<ListGraphType<LinkedIssueType>>("children",
            resolve: context =>
                IssueModelType.ResolveIssueChildren(
                    context.Source.Id,
                    context.RequestServices?.GetService<IIssueRepository>(),
                    context.CancellationToken));
    }

}
