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

using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.State;

namespace IssueTracker.Issues.API.GRPC.Extensions;


internal static class GrpcEnumConverterExtensions
{

    public static Priority? ToDomainOrNull(this GrpcProto.Priority source)
    {
        int value = (int)source - 1;

        if (value < 0)
        {
            return null;
        }

        return (Priority)value;
    }

    public static IssueType? ToDomainOrNull(this GrpcProto.IssueType source)
    {
        int value = (int)source - 1;

        if (value < 0)
        {
            return null;
        }

        return (IssueType)value;
    }

    public static IssueStateValue? ToDomainOrNull(this GrpcProto.IssueState source)
    {
        int value = (int)source - 1;

        if (value < 0)
        {
            return null;
        }

        return (IssueStateValue)value;
    }

    public static Priority ToDomainOrDefault(this GrpcProto.Priority source)
    {
        int value = (int)source - 1;

        if (value < 0)
        {
            return default;
        }

        return (Priority)value;
    }

    public static IssueType ToDomainOrDefault(this GrpcProto.IssueType source)
    {
        int value = (int)source - 1;

        if (value < 0)
        {
            return default;
        }

        return (IssueType)value;
    }

    public static IssueStateValue ToDomainOrDefault(this GrpcProto.IssueState source)
    {
        int value = (int)source - 1;

        if (value < 0)
        {
            return default;
        }

        return (IssueStateValue)value;
    }

    public static GrpcProto.Priority ToDto(this Priority source) =>
        (GrpcProto.Priority)((int)source + 1);

    public static GrpcProto.IssueType ToDto(this IssueType source) =>
        (GrpcProto.IssueType)((int)source + 1);

    public static GrpcProto.IssueState ToDto(this IssueStateValue source) =>
        (GrpcProto.IssueState)((int)source + 1);
}
