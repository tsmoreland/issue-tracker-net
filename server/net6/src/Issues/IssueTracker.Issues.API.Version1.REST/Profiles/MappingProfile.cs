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
using Source = IssueTracker.Issues.API.Version1.Abstractions.DataTransferObjects;
using Destination = IssueTracker.Issues.API.Version1.REST.DataTransferObjects;

namespace IssueTracker.Issues.API.Version1.REST.Profiles;

/// <summary>
/// Automappper configuration mapping API DTOs to REST DTOs
/// </summary>
public sealed class MappingProfile : Profile
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="MappingProfile"/> class.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<Source.IssueDto, Destination.Response.IssueDto>()
            .ReverseMap();

        CreateMap<Source.IssueSummaryDto, Destination.Response.IssueSummaryDto>()
            .ReverseMap();

        CreateMap<Source.IssueSummaryPage, Destination.Response.IssueSummaryPage>()
            .ReverseMap();
    }
}
