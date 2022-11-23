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

using ProtoBuf;

namespace IssueTracker.Issues.API.GRPCNet.Messages.Response;

[ProtoContract]
public sealed class IssueMessage
{
    [ProtoMember(1)]
    public ResultCode Status { get; set; } = default;

    [ProtoMember(2)]
    public string Id { get; set; } = string.Empty;

    [ProtoMember(3)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Short description of the issue
    /// </summary>
    /// <example>Sample Description</example>
    [ProtoMember(4)]
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// Issue Priority
    /// </summary>
    /// <example>Low</example>
    [ProtoMember(5)]
    public Priority Priority { get; set; } = Priority.Low;

    /// <summary>
    /// Issue type
    /// </summary>
    /// <example>Defect</example>
    [ProtoMember(6)]
    public IssueType Type { get; set; } = IssueType.Defect;

    /// <summary>
    /// Issue State
    /// </summary>
    /// <example>BackLog</example>
    [ProtoMember(7)]
    public IssueState State { get; set; } = IssueState.Backlog;

    /// <summary>
    /// Reporter
    /// </summary>
    [ProtoMember(8)]
    public UserMessage? Reporter { get; set; } 

    /// <summary>
    /// Assigned maintainer
    /// </summary>
    [ProtoMember(9)]
    public UserMessage? Assignee { get; set; } 

    /// <summary>
    /// Epic Id
    /// </summary>
    /// <example>APP-1</example>
    [ProtoMember(10)]
    public string? EpicId { get; set; } = null;

    public static IssueMessage NotFound() =>
        new() { Status = ResultCode.NotFound };
}
