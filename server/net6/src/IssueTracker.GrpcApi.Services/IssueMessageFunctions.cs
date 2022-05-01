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

using IssueTracker.Core.Model;
using IssueTracker.Core.ValueObjects;
using IssueTracker.GrpcApi.Grpc.Services;

namespace IssueTracker.GrpcApi.Services;

internal static class IssueMessageFunctions
{
    private static readonly UserMessage s_userFlyweight = new()
    {
        Id = User.Unassigned.Id.ToString(),
        FullName = User.Unassigned.FullName,
    };

    private static readonly IssueMessage s_flyweight = new ()
    {
        Id = Guid.Empty.ToString(),
        Title = string.Empty,
        Description = string.Empty,
        Priority = (int)Priority.Low,
        Type = (int)IssueType.Defect,
        Assignee = s_userFlyweight,
        Reporter = s_userFlyweight,
    };

    public static IssueMessage InvalidArgument()
    {
        return new IssueMessage
        {
            Status = ResultCode.InvalidArgument,
            Id = s_flyweight.Id,
            Title = s_flyweight.Title,
            Description = s_flyweight.Description,
            Priority = s_flyweight.Priority,
            Type = s_flyweight.Type,
        };
    }
    public static IssueMessage NotFound()
    {
        return new IssueMessage
        {
            Status = ResultCode.NotFound,
            Id = s_flyweight.Id,
            Title = s_flyweight.Title,
            Description = s_flyweight.Description,
            Priority = s_flyweight.Priority,
            Type = s_flyweight.Type,
            Assignee = s_userFlyweight,
            Reporter = s_userFlyweight,
        };
    }

    public static UserMessage ToMessage(this User user)
    {
        return new UserMessage { Id = user.Id.ToString(), FullName = user.FullName };
    }

    public static IssueMessage ToMessage(this Issue issue)
    {
        return new IssueMessage
        {
            Status = ResultCode.Success,
            Id = issue.Id.ToString(),
            Title = issue.Title,
            Description = issue.Description,
            Priority = (int)issue.Priority,
            Type = (int)issue.Type,
            Assignee = issue.Assignee.ToMessage(),
            Reporter = issue.Reporter.ToMessage(),
        };
    }

    public static User ToUser(this UserMessage message)
    {
        return new User(Guid.Parse(message.Id), message.FullName);
    }

    public static Issue ToIssue(this AddIssueMessage message)
    {
        return new Issue(
            message.Project,
            message.Title,
            message.Description,
            (Priority)message.Priority,
            (IssueType)message.Type,
            message.Assignee.ToUser(),
            message.Reporter.ToUser());
    }
    public static void UpdateIssue(this EditIssueMessage message, Issue issue)
    {
        issue.SetTitle(message.Title);
        issue.SetDescription(message.Description);
        issue.SetPriority((Priority)message.Priority);
        issue.SetIssueType((IssueType)message.Type);
        issue.SetAssignee(message.Assignee.ToUser());
        issue.SetReporter(message.Reporter.ToUser());
    }
}
