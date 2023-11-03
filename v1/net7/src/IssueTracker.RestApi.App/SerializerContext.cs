// 
// Copyright (c) 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Text.Json.Serialization;
using IssueTracker.Issues.API.REST.VersionIndependent.DataTransferObjects.Response;
using ResponseV2 = IssueTracker.Issues.API.REST.Version2.DataTransferObjects.Response;

namespace IssueTracker.RestApi.App;

/// <summary/>
[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Default)]
[JsonSerializable(typeof(ResponseV2.IssueDto))]
[JsonSerializable(typeof(ResponseV2.IssueSummaryPage))]
[JsonSerializable(typeof(ResponseV2.IssueSummaryDto))]
[JsonSerializable(typeof(ResponseV2.LinkedIssueSummaryPage))]
[JsonSerializable(typeof(ResponseV2.LinkedIssueSummaryDto))]
[JsonSerializable(typeof(ResponseV2.UserDto))]
[JsonSerializable(typeof(ResponseV2.IssueSummaryPageWithLinks))]
[JsonSerializable(typeof(ResponseV2.IssueDtoWithLinks))]
[JsonSerializable(typeof(LinkDto))]
public partial class SerializerContext : JsonSerializerContext
{
}
