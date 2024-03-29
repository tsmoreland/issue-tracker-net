﻿//
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

using IssueTracker.Issues.API.GRPC.Proto;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.Services.Version2.DataTransferObjects;

namespace IssueTracker.Issues.API.GRPC;

internal static class IssueMessageFunctions
{
    private static readonly UserMessage s_userFlyweight = new()
    {
        UserId = User.Unassigned.UserId.ToString(),
        FullName = User.Unassigned.FullName,
    };

    private static readonly IssueMessage s_flyweight = new()
    {
        Id = Guid.Empty.ToString(),
        Title = string.Empty,
        Description = string.Empty,
        State = default,
        Priority = default,
        Type = default,
        Assignee = s_userFlyweight,
        Reporter = s_userFlyweight,
        EpicId = EmptyOptionalString(),
    };

    private static IssueMessage FlyweightWithResult(ResultCode result)
    {
        return new IssueMessage
        {
            Status = result,
            Id = s_flyweight.Id,
            Title = s_flyweight.Title,
            Description = s_flyweight.Description,
            Priority = s_flyweight.Priority,
            Type = s_flyweight.Type,
            State = s_flyweight.State,
            Assignee = s_userFlyweight,
            Reporter = s_userFlyweight,
            EpicId = s_flyweight.EpicId,
        };
    }

    public static OptionalString EmptyOptionalString() => new() { HasValue = false, Value = "" };

    public static IssueMessage InvalidArgument()
    {
        return FlyweightWithResult(ResultCode.ResultInvalidArgument);
    }
    public static IssueMessage NotFound()
    {
        return FlyweightWithResult(ResultCode.ResultNotFound);
    }

    public static UserMessage ToMessage(this UserDto? user)
    {
        return user is not null
            ? new UserMessage { UserId = user.Id.ToString(), FullName = user.FullName }
            : new UserMessage { UserId = User.Unassigned.UserId.ToString(), FullName = User.Unassigned.FullName };
    }

    public static IssueMessage ToMessage(this IssueDto issue)
    {
        return new IssueMessage
        {
            Status = ResultCode.ResultSuccess,
            Id = issue.Id.ToString(),
            Title = issue.Title,
            Description = issue.Description,
            Priority = issue.Priority.ToDto(),
            State = issue.State.ToDto(),
            Type = issue.Type.ToDto(),
            Assignee = issue.Assignee.ToMessage(),
            Reporter = issue.Reporter.ToMessage(),
        };
    }

    public static User ToUser(this UserMessage message)
    {
        return new User(Guid.Parse(message.UserId), message.FullName);
    }
}
