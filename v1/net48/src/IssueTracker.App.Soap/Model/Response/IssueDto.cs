﻿//
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

using System;
using System.Runtime.Serialization;
using IssueTracker.Core.Model;

namespace IssueTracker.App.Soap.Model.Response
{
    /// <summary>
    /// Issue Data Transfer object
    /// </summary>
    [DataContract]
    public sealed class IssueDto
    {

        /// <summary>
        /// Instantiates a new instance of the <see cref="IssueDto"/> class.
        /// </summary>
        public IssueDto(Guid id, string title, string description, Priority priority)
        {
            Id = id;
            Title = title;
            Description = description;
            Priority = priority;
        }

        /// <summary>
        /// Instantiates a new instance of the <see cref="IssueDto"/> class.
        /// </summary>
        /// <remarks>
        /// required for XML serialization, along with public setters
        /// </remarks>
        public IssueDto()
        {
            Id = Guid.Empty;
            Title = string.Empty;
            Description = string.Empty;
            Priority = Priority.Low;
        }

        /// <summary>
        /// Issue Id
        /// </summary>
        /// <example>48EE3BF9-C81D-4FE4-AB02-220C0122AFE4</example>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Issue Title
        /// </summary>
        /// <example>Example Title</example>
        [DataMember]
        public string Title { get; set; } 

        /// <summary>
        /// Issue Description
        /// </summary>
        /// <example>Example Description</example>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Issue Priority
        /// </summary>
        /// <example>Medium</example>
        [DataMember]
        public Priority Priority { get; set; }

        /// <summary>
        /// Converts <see cref="IssueDto"/> to <see cref="IssueDto"/>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IssueDto From(Issue model)
        {
            return model is null
                ? null
                : new IssueDto(model.Id, model.Title, model.Description, model.Priority);
        }
    }
}
