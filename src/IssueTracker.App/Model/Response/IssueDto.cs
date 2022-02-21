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

namespace IssueTracker.App.Model.Response;

/// <summary>
/// Issue Details
/// </summary>
/// <param name="Id">Issue Id</param>
/// <param name="Title">Issue Title</param>
/// <param name="Description">Issue description</param>
/// <param name="Priority">issue priority</param>
public record class IssueDto(Guid Id, string Title, string Description, Priority Priority)
{
    /// <summary>
    /// Issue Id
    /// </summary>
    public Guid Id { get; init; } = Id;
    /// <summary>
    /// Issue Title
    /// </summary>
    public string Title { get; init; } = Title;

    /// <summary>
    /// Issue Description
    /// </summary>
    public string Description { get; init; } = Description;

    /// <summary>
    /// Issue Priority
    /// </summary>
    public Priority Priority { get; init; } = Priority;

    /// <summary>
    /// Converts <see cref="Issue"/> to <see cref="IssueDto"/>
    /// </summary>
    public static IssueDto FromIssue(Issue issue)
    {
        return new IssueDto(issue.Id, issue.Title, issue.Description, issue.Priority);
    }
}
