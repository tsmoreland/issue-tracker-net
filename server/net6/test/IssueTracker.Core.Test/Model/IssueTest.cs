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
using IssueTracker.Core.ValueObjects;

namespace IssueTracker.Core.Test.Model;

public sealed class IssueTest
{
    private const string ProjectId = "APP";
    private const string Title = "Issue";
    private const string Description = "empty";
    private static readonly Priority s_priority = Priority.Low;


    [TestCase(null)]
    [TestCase("")]
    public void SetDescription_StoresEmptyString_WhenValueIsNullOrEmpt(string value)
    {
        Issue issue = new(ProjectId, Title, Description, s_priority);
        issue.SetDescription(value);
        Assert.That(issue.Description, Is.Empty);
    }

    [Test]
    public void Assignee_ReturnsNull_WhenPrivateSetterUsedViaReflection()
    {
        Issue issue = new(ProjectId, Title, Description, s_priority);
        PropertyInfo? property = issue.GetType().GetProperty(nameof(Issue.Assignee));
        property?.SetValue(issue, null);
        Assert.That(issue.Assignee, Is.Null);
    }

    [Test]
    public void Reporter_ReturnsNull_WhenPrivateSetterUsedViaReflection()
    {
        Issue issue = new(ProjectId, Title, Description, s_priority);
        PropertyInfo? property = issue.GetType().GetProperty(nameof(Issue.Reporter));
        property?.SetValue(issue, null);
        Assert.That(issue.Reporter, Is.Null);
    }
}
