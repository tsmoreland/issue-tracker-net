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

using AutoMapper;
using IssueTracker.Core.Model;
using IssueTracker.Core.Views;
using IssueTracker.GraphQl.App.Model.Response;

namespace IssueTracker.GraphQl.App.Profiles;

/// <summary>
/// Auto Mapper Profile
/// </summary>
public sealed class MappingProfile : Profile
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="MappingProfile"/> class.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<LinkedIssueView, LinkedIssueViewDto>()
            .ConstructUsing((i, ctx) =>
                new LinkedIssueViewDto(
                    i.LinkType,
                    i.IssueId.ToString(),
                    i.Title,
                    i.Description,
                    i.Priority,
                    i.Type,
                    i.ParentIssues.Select(pi => ctx.Mapper.Map<LinkedIssueViewDto>(pi)).ToList(),
                    i.ChildIssues.Select(ci => ctx.Mapper.Map<LinkedIssueViewDto>(ci)).ToList(),
                    i.Assignee.FullName,
                    i.Reporter.FullName));

        CreateMap<Issue, IssueDto>()
            .ConstructUsing((i, ctx) =>
                new IssueDto(
                    i.IssueId.ToString(),
                    i.Title,
                    i.Description,
                    i.Priority,
                    i.Type,
                    i.ParentIssues.Select(pi => ctx.Mapper.Map<LinkedIssueViewDto>(pi)).ToList(),
                    i.ChildIssues.Select(ci => ctx.Mapper.Map<LinkedIssueViewDto>(ci)).ToList(),
                    i.Assignee.FullName,
                    i.Reporter.FullName));

    }
}
