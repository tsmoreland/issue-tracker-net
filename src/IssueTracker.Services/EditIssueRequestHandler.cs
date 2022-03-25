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

using IssueTracker.Core.Model;
using IssueTracker.Data.Abstractions;
using IssueTracker.Services.Abstractions.Model.Request;
using IssueTracker.Services.Abstractions.Model.Response;
using IssueTracker.Services.Abstractions.Requests;
using MediatR;

namespace IssueTracker.Services;

public sealed class EditIssueRequestHandler : IRequestHandler<EditIssueRequest, IssueDto?>
{
    private readonly IIssueRepository _repository;

    public EditIssueRequestHandler(IIssueRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public Task<IssueDto?> Handle(EditIssueRequest request, CancellationToken cancellationToken)
    {
        (Guid id, EditIssueDto model) = request;
        return _repository.GetIssueById(id, cancellationToken)
            .ContinueWith(t =>
            {
                Issue? issue = t.Result;
                t.Dispose();
                if (issue == null)
                {
                    return null;
                }

                issue.CopyTopLevelFrom(model.ToModel());
                return IssueDto.FromIssue(issue);

            }, TaskContinuationOptions.OnlyOnRanToCompletion);
    }
}
