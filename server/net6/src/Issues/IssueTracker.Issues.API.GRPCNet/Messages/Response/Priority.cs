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
public enum Priority
{
    [EnumMember]
    [ProtoEnum]
    None = 0,

    [EnumMember]
    [ProtoEnum]
    Low = 1,

    [EnumMember]
    [ProtoEnum]
    Medium = 2,

    [EnumMember]
    [ProtoEnum]
    High = 3,
}

public static class PriorityExtensions
{
    public static Domain.ModelAggregates.IssueAggregate.Priority ToDomainOrDefault(this Priority source)
    {
        return source == Priority.None
            ? default
            : (Domain.ModelAggregates.IssueAggregate.Priority)((int)source - 1);

    }
    public static Domain.ModelAggregates.IssueAggregate.Priority? ToDomainOrNull(this Priority source)
    {
        return source == Priority.None
            ? null
            : (Domain.ModelAggregates.IssueAggregate.Priority)((int)source - 1);
    }

    public static Priority ToMessage(this Domain.ModelAggregates.IssueAggregate.Priority source)
    {
        return (Priority)((int)source + 1);
    }
}
