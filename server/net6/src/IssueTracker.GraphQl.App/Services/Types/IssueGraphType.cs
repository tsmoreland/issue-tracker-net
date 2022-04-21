﻿//
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

using GraphQL.Types;
using IssueTracker.Core.Model;

namespace IssueTracker.GraphQl.App.Services.Types;

/// <summary>
/// <see cref="Issue"/> Object Graph
/// </summary>
public sealed class IssueGraphType : ObjectGraphType<Issue>
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="IssueGraphType"/> class
    /// configuring its fields
    /// </summary>
    public IssueGraphType()
    {
        Field(type => type.Id, nullable: false).Description("Unique Id");
        Field(type => type.Title, nullable: false).Description("Title");
        Field(type => type.Description).Description("description of the issue");
        Field(type => type.Priority, nullable: false).Description("Resolution Priority");
        Field(type => type.Type, nullable: false).Description("issue category or type");

    }
}
