//
// Copyright (c) 2022 Terry Moreland
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
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;

namespace IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Request;

/// <summary>
/// Issue DTO used for patch requests
/// </summary>
public sealed class IssuePatch
{

    private string _title = string.Empty;
    private string _description = string.Empty;
    private string? _epicId;
    private IssueType _type = IssueType.Defect;
    private Guid? _assignee;
    private Guid? _reporter;
    private ICollection<IssueLinkPatch> _children = new HashSet<IssueLinkPatch>();
    private ICollection<IssueLinkPatch> _parents = new HashSet<IssueLinkPatch>();
    private DateTimeOffset? _startTime;
    private DateTimeOffset? _stopTime;

    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    internal HashSet<string> ModifiedFields { get; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="IssuePatch"/> class.
    /// </summary>
    public IssuePatch()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IssuePatch"/> class,
    /// copying values from <paramref name="issue"/>
    /// </summary>
    [return: NotNullIfNotNull(nameof(Issue))]
    public static IssuePatch? FromIssue(Issue? issue)
    {
        if (issue is null)
        {
            return null;
        }

        return new IssuePatch
        {
            _title = issue.Title,
            _description = issue.Description,
            _epicId = issue.EpicId?.ToString(),
            _type = issue.Type,
            _assignee = issue.Assignee is not null && issue.Assignee != User.Unassigned ? issue.Assignee.UserId : null,
            _reporter = issue.Reporter is not null && issue.Reporter != User.Unassigned ? issue.Reporter.UserId : null,
            // TODO
            //  need related to and from separately from issue, need new properties which are configured to be ignored
            //  by EF
            _startTime = issue.StartTime,
            _stopTime = issue.StopTime,
        };
    }

    public IReadOnlyDictionary<string, object?> GetChanges()
    {
        Dictionary<string, object?> changes = new();

        foreach (string name in ModifiedFields)
        {
            changes[name] = name switch
            {
                nameof(Title) => Title,
                nameof(Description) => Description,
                nameof(EpicId) => EpicId,
                nameof(Type) => Type,
                nameof(Assignee) => Assignee,
                nameof(Reporter) => Reporter,
                nameof(StartTime) => StartTime,
                nameof(StopTime) => StopTime,
                _ => throw new InvalidOperationException($"unrecognized field name {name}"),
            };
        }
        return changes;
    }


    public string Title
    {
        get => _title;
        set
        {
            ModifiedFields.Add(nameof(Title));
            _title = value;
        }
    }
    public string Description
    {
        get => _description;
        set
        {
            ModifiedFields.Add(nameof(Description));
            _description = value;
        }
    }

    public string? EpicId
    {
        get => _epicId;
        set
        {
            ModifiedFields.Add(nameof(EpicId));
            _epicId = value;
        }
    }

    public IssueType Type
    {
        get => _type;
        set
        {
            ModifiedFields.Add(nameof(Type));
            _type = value;
        }
    }
    public Guid? Assignee
    {
        get => _assignee;
        set
        {
            ModifiedFields.Add(nameof(Assignee));
            _assignee = value;
        }
    }
    public Guid? Reporter
    {
        get => _reporter;
        set
        {
            ModifiedFields.Add(nameof(Reporter));
            _reporter = value;
        }
    }
    public ICollection<IssueLinkPatch> Children
    {
        get => _children;
        set
        {
            ModifiedFields.Add(nameof(Children));
            _children = value;
        }
    }
    public ICollection<IssueLinkPatch> Parents
    {
        get => _parents;
        set
        {
            ModifiedFields.Add(nameof(Parents));
            _parents = value;
        }
    }

    public DateTimeOffset? StartTime
    {
        get => _startTime;
        set
        {
            ModifiedFields.Add(nameof(StartTime));
            _startTime = value;
        }
    }
    public DateTimeOffset? StopTime
    {
        get => _stopTime;
        set
        {
            ModifiedFields.Add(nameof(StopTime));
            _stopTime = value;
        }
    }
}
