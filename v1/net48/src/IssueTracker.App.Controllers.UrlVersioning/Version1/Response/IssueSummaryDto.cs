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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Core.Projections;
using IssueTracker.SwashBuckleExtensions.Abstractions;

namespace IssueTracker.App.Controllers.UrlVersioning.Version1.Response
{
    /// <summary>
    /// Issue Short Summary, intended for bulk retrieval
    /// </summary>
    [SwaggerSchemaName("Issue Summary")]
    public sealed class IssueSummaryDto
    {
        /// <summary>
        /// Instantiates a new instance of he IssueSummaryDto Class
        /// </summary>
        public IssueSummaryDto(Guid id, string title)
        {
            Id = id;
            Title = title;
        }

        /// <summary>
        /// Instantiates a new instance of he IssueSummaryDto Class
        /// </summary>
        public IssueSummaryDto()
        {
            Id = Guid.Empty;
            Title = string.Empty;
        }

        /// <summary>
        /// Issue Id
        /// </summary>
        /// <example>3C1152EC-DC0C-4AB0-8AF9-10DE5A9705D5</example>
        [Required]
        public Guid Id { get; private set; } 

        /// <summary>
        /// Issue Title
        /// </summary>
        /// <example>Example Title</example>
        [Required]
        public string Title { get; private set; }

        /// <summary>
        /// Converts service DTO to versioned API DTO, for latest version of API the class is expected to be equivalent
        /// </summary>
        public static IssueSummaryDto FromProjection(IssueSummaryProjection model)
        {
            (Guid id, string name, _, _) = model;
            return new IssueSummaryDto(id, name);
        }

        /// <summary>
        /// Converts an asynchronous collection of <see cref="IssueSummaryDto"/> to
        /// an asynchronous colleciton of <see cref="IssueSummaryDto"/>
        /// </summary>
        public static async IAsyncEnumerable<IssueSummaryDto> MapFrom(
            IAsyncEnumerable<IssueSummaryProjection> source, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (IssueSummaryProjection issueSummary in source.WithCancellation(cancellationToken))
            {
                yield return FromProjection(issueSummary);
            }
        }
    }
}
