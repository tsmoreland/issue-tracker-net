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

using System.Reflection;
using IssueTracker.Core.Model;
using IssueTracker.Core.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssueTracker.Data;

internal static class SeedData
{
    public static void HasData(EntityTypeBuilder<Issue> issueEntity, EntityTypeBuilder<LinkedIssue> linkedIssueEntity)
    {
        PropertyInfo? assigneeSet = typeof(Issue).GetProperty(nameof(Issue.Assignee));
        PropertyInfo? reporterSet = typeof(Issue).GetProperty(nameof(Issue.Reporter));

        if (assigneeSet is null || reporterSet is null)
        {
            throw new InvalidOperationException("Unable to access private setter");
        }

        // adding owned types would use something like:
        #if EXAMPLE_CODE // resharper doesn't complain about #if blocks, but does commented out code
        issueEntity.OwnsOne(e => e.Reporter)
            .HasData(new
            {
                IssueIdentifier = "id of the issue entity", // this is the shadow property created by EF to link the objects, Issue matches the class name of the owning entity
                UserId = "id property of the user object",
                FullName = "fullname property from user table"
            });
        #endif
        // or so some internal documentation hinted, when we do it it expects a User object which doesn't leave room for the shadow property UserId

        Issue epic = new(
            "APP", 1,
            "Add Project support", "add projects and use project id as link between issue and project",
            Priority.High, IssueType.Epic,
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>(),
            TriageUser.Unassigned,
            Maintainer.Unassigned,
            null,
            new DateTime(2022, 01, 01, 12, 30, 0, DateTimeKind.Utc),
            Guid.NewGuid().ToString());
        assigneeSet.SetValue(epic, null);
        reporterSet.SetValue(epic, null);

        Issue dataStory = new(
            "APP", 2,
            "The database should story issues with links to projects", "",
            Priority.Medium, IssueType.Story,
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>(),
            TriageUser.Unassigned,
            Maintainer.Unassigned,
            null,
            new DateTime(2022, 01, 01, 12, 40, 0, DateTimeKind.Utc),
            Guid.NewGuid().ToString());
        assigneeSet.SetValue(dataStory, null);
        reporterSet.SetValue(dataStory, null);

        Issue queryStory = new(
            "APP", 3,
            "The api should be able to retrieve projects", "As a user I want to be able to retreive all projects",
            Priority.Low, IssueType.Story,
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>(),
            TriageUser.Unassigned,
            Maintainer.Unassigned,
            null,
            new DateTime(2022, 01, 01, 12, 45, 0, DateTimeKind.Utc),
            Guid.NewGuid().ToString());
        assigneeSet.SetValue(queryStory, null);
        reporterSet.SetValue(queryStory, null);

        Issue dataTask = new(
            "APP", 4,
            "add project core models", "add the model(s) for project type ensuring it's id matches the expectations of issue",
            Priority.Medium, IssueType.Task,
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>(),
            TriageUser.Unassigned,
            Maintainer.Unassigned,
            null,
            new DateTime(2022, 01, 02, 12, 0, 0, DateTimeKind.Utc),
            Guid.NewGuid().ToString());
        assigneeSet.SetValue(dataTask, null);
        reporterSet.SetValue(dataTask, null);

        Issue queryTask = new(
            "APP", 5,
            "add mediator request/handlers for project query", "add the request handlers to get projects by id and a summary",
            Priority.Low, IssueType.Task,
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>(),
            TriageUser.Unassigned,
            Maintainer.Unassigned,
            null,
            new DateTime(2022, 01, 03, 13, 45, 0, DateTimeKind.Utc),
            Guid.NewGuid().ToString());
        assigneeSet.SetValue(queryTask, null);
        reporterSet.SetValue(queryTask, null);

        issueEntity.HasData(epic, dataStory, queryStory, dataTask, queryTask);

        LinkedIssue taskLink = new(LinkType.Blocking, dataTask.IssueId, dataTask.Id, queryTask.IssueId, queryTask.Id, Guid.NewGuid().ToString());
        LinkedIssue storyLink = new(LinkType.Related, dataStory.IssueId, dataStory.Id, queryStory.IssueId, queryStory.Id, Guid.NewGuid().ToString());
        linkedIssueEntity.HasData(taskLink, storyLink);
    }
}
