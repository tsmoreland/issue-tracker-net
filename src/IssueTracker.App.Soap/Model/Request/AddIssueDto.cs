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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using IssueTracker.Core.Model;

namespace IssueTracker.App.Soap.Model.Request
{
    /// <summary>
    /// Add Issue Data Transfer Object
    /// </summary>
    public class AddIssueDto
    {
        /// <summary>
        /// instantiates a new instance of the <see cref="AddIssueDto"/> class.
        /// </summary>
        public AddIssueDto(string title, string description, Priority priority)
        {
            Title = title;
            Description = description;
            Priority = priority;
        }

        public AddIssueDto()
        {
            Title = string.Empty;
            Description = string.Empty;
            Priority = Priority.Low;
        }

        /// <summary>
        /// Issue Title
        /// </summary>
        /// <example>Example Title</example>
        [DataMember]
        public string Title { get; private set; } 

        /// <summary>
        /// Issue Description
        /// </summary>
        /// <example>Example description</example>
        [DataMember]
        public string Description { get; private set; } 

        /// <summary>
        /// Issue Priority
        /// </summary>
        /// <example>High</example>
        [DataMember]
        public Priority Priority { get; private set; } 

        /// <summary>
        /// Converts DTO to Model
        /// </summary>
        /// <returns>Model</returns>
        public Issue ToModel()
        {
            return new Issue(Title, Description, Priority, IssueType.Defect);
        }
    }
}
