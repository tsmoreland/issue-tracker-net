//
// Copyright (c) 2023 Terry Moreland
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

using System.Diagnostics.CodeAnalysis;
using IssueTracker.Issues.API.GRPC.Proto;
using IssueTracker.Issues.Domain.Services.Version2.DataTransferObjects;

namespace IssueTracker.Issues.API.GRPC;

internal static class IssueSummariesMessageFactory
{
    public static bool IsValid(this PagedIssueRequestMessage message, [NotNullWhen(false)] out IssueSummariesMessage? error)
    {
        (int pageNumber, int pageSize) = (message.PageNumber, message.PageSize);

        if (pageNumber <= 0)
        {
            error = InvalidArgument();
            return false;
        }

        if (pageSize <= 0)
        {
            error = InvalidArgument();
            return false;
        }

        error = null;
        return true;
    }

    public static IssueSummariesMessage InvalidArgument()
    {
        return new IssueSummariesMessage { Status = ResultCode.ResultInvalidArgument };
    }

    public static IssueSummaryMessage ToMessage(this IssueSummaryDto source)
    {
        return new IssueSummaryMessage
        {
            Id = source.Id,
            Title = source.Title,
            Priority = source.Priority.ToDto(),
            Type = source.Type.ToDto()
        };
    }

}
