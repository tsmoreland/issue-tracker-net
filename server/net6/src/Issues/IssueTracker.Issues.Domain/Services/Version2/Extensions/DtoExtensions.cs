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

using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Projections;
using IssueTracker.Issues.Domain.Services.Version2.DataTransferObjects;

namespace IssueTracker.Issues.Domain.Services.Version2.Extensions;

internal static class DtoExtensions
{
    public static IssueDto ToDto(this Issue source)
    {
        return new IssueDto(
            source.Id.ToString(),
            source.Title,
            source.Description,
            source.Priority,
            source.Type,
            source.State.Value,
            source.Reporter.ToDto(),
            source.Assignee.ToDto(),
            source.EpicId?.ToString(),
            Array.Empty<LinkedIssueSummaryDto>());
    }

    public static TriageUserDto ToDto(this TriageUser source)
    {
        return new TriageUserDto(source.UserId, source.FullName);
    }
    public static MaintainerDto ToDto(this Maintainer source)
    {
        return new MaintainerDto(source.UserId, source.FullName);
    }

    public static IAsyncEnumerable<IssueSummaryDto> ToDto(this IAsyncEnumerable<IssueSummaryProjection> summaries)
    {
        return summaries
            .Select(i => new IssueSummaryDto(i.Id.ToString(), i.Title, i.Priority, i.Type));
    }
}
