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
    private static readonly TriageUserMessage s_triageUserFlyweight = new()
    {
        UserId = TriageUser.Unassigned.UserId.ToString(),
        FullName = TriageUser.Unassigned.FullName,
    };
    private static readonly MaintainerMessage s_maintainerFlyweight = new()
    {
        UserId = TriageUser.Unassigned.UserId.ToString(),
        FullName = TriageUser.Unassigned.FullName,
    };

    private static readonly IssueMessage s_flyweight = new ()
    {
        Id = Guid.Empty.ToString(),
        Title = string.Empty,
        Description = string.Empty,
        Priority = (int)Priority.Low,
        Type = (int)IssueType.Defect,
        Assignee = s_maintainerFlyweight,
        Reporter = s_triageUserFlyweight,
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
            Assignee = s_maintainerFlyweight,
            Reporter = s_triageUserFlyweight,
        };
    }

    public static TriageUserMessage ToMessage(this TriageUser user)
    {
        return new TriageUserMessage { UserId = user.UserId.ToString(), FullName = user.FullName };
    }
    public static MaintainerMessage ToMessage(this Maintainer user)
    {
        return new MaintainerMessage { UserId = user.UserId.ToString(), FullName = user.FullName };
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

    public static TriageUser ToTriageUser(this TriageUserMessage message)
    {
        return new TriageUser(Guid.Parse(message.UserId), message.FullName);
    }
    public static Maintainer ToMaintainer(this MaintainerMessage message)
    {
        return new Maintainer(Guid.Parse(message.UserId), message.FullName);
    }

    public static Issue ToIssue(this AddIssueMessage message)
    {
        return new Issue(
            message.Project,
            message.Title,
            message.Description,
            (Priority)message.Priority,
            (IssueType)message.Type,
            message.Reporter.ToTriageUser(),
            message.Assignee.ToMaintainer(),
            null); // TODO
    }
    public static void UpdateIssue(this EditIssueMessage message, Issue issue)
    {
        issue.SetTitle(message.Title);
        issue.SetDescription(message.Description);
        issue.SetPriority((Priority)message.Priority);
        issue.SetIssueType((IssueType)message.Type);
        issue.SetReporter(message.Reporter.ToTriageUser());
        issue.SetAssignee(message.Assignee.ToMaintainer());
    }
}
