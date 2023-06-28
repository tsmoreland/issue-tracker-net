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

using System.ComponentModel.DataAnnotations;
using IssueTracker.Core.Model;

namespace IssueTracker.RestApi.Shared.Model.Response;

public sealed class IssueV2 
{

    public IssueV2(int id, string title, string description, Priority priority,  IssueType type)
    {
        Id = id;
        Title = title;
        Description = description;
        Priority = priority;
        Type = type;
    }

    /// <summary>
    /// Instantiates a new instance of the <see cref="IssueDto"/> class.
    /// </summary>
    /// <remarks>
    /// required for XML serialization, along with public setters
    /// </remarks>
    public IssueV2()
    {
        Id = 0;
        Title = string.Empty;
        Description = string.Empty;
        Priority = Priority.Low;
        Type = IssueType.Defect;
    }

    /// <summary>
    /// Issue Id
    /// </summary>
    /// <example>48</example>
    [Required]
    public int Id { get; set; }

    /// <summary>
    /// Issue Title
    /// </summary>
    /// <example>Example Title</example>
    [Required]
    public string Title { get; set; } 

    /// <summary>
    /// Issue Description
    /// </summary>
    /// <example>Example Description</example>
    [Required]
    public string Description { get; set; }

    /// <summary>
    /// Issue Priority
    /// </summary>
    /// <example>Medium</example>
    [Required]
    public Priority Priority { get; set; }

    /// <summary>
    /// Issue Type
    /// </summary>
    [Required]
    public IssueType Type { get; }


    /// <summary>
    /// Converts <see cref="Issue"/> to <see cref="IssueV1"/>
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public static IssueV2 From(Issue model)
    {
        return model is null
            ? null
            : new IssueV2(model.Id, model.Title, model.Description, model.Priority, model.Type);
    }
}
