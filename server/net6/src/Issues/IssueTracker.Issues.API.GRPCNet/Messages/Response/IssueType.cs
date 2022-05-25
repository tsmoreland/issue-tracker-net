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

using System.Runtime.Serialization;
using ProtoBuf;

namespace IssueTracker.Issues.API.GRPCNet.Messages.Response;

[ProtoContract]
public enum IssueType
{
    [EnumMember]
    [ProtoEnum]
    None = 0,

    [EnumMember]
    [ProtoEnum]
    Epic = 1,

    [EnumMember]
    [ProtoEnum]
    Story = 2,

    [EnumMember]
    [ProtoEnum]
    Task = 3,

    [EnumMember]
    [ProtoEnum]
    SubTask  = 4,

    [EnumMember]
    [ProtoEnum]
    Defect = 5,
}

public static class IssueTypeExtensions
{
    public static Domain.ModelAggregates.IssueAggregate.IssueType ToDomainOrDefault(this IssueType source)
    {
        return source == IssueType.None
            ? default
            : (Domain.ModelAggregates.IssueAggregate.IssueType)((int)source - 1);

    }
    public static Domain.ModelAggregates.IssueAggregate.IssueType? ToDomainOrNull(this IssueType source)
    {
        return source == IssueType.None
            ? null
            : (Domain.ModelAggregates.IssueAggregate.IssueType)((int)source - 1);
    }

    public static IssueType ToMessage(this Domain.ModelAggregates.IssueAggregate.IssueType source)
    {
        return (IssueType)((int)source + 1);
    }
}
