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
//

using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using IssueTracker.Issues.Domain.Services.Version2.Commands;
using IssueTracker.Issues.Domain.Services.Version2.DataTransferObjects;
using IssueTracker.Issues.Domain.Services.Version2.Extensions;
using MediatR;

namespace IssueTracker.Issues.Domain.Services.Version2.CommandHandlers;

public sealed class PatchIssueCommandHandler : IRequestHandler<PatchIssueCommand, IssueDto?>
{
    private readonly IIssueRepository _repository;

    public PatchIssueCommandHandler(IIssueRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc />
    public async Task<IssueDto?> Handle(PatchIssueCommand request, CancellationToken cancellationToken)
    {
        (IssueIdentifier id, IReadOnlyDictionary<string, object?> modifications) = request;

        // this should be the same value since it'll be the same repository
        Issue? issue = await _repository.GetByIdOrDefault(id, true, cancellationToken);
        if (issue is null)
        {
            return null;
        }

        foreach ((string key, object? value) in modifications)
        {
            if (key is nameof(Issue.Assignee))
            {
                // convert value to Maintainer by requesting the name from somewhere
            }
            else if (key is nameof(Issue.Reporter))
            {
                // convert value to TriageUser by requesting the name from somewhere
            }


            issue.PatchOrThrow(key, value);
        }

        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return issue.ToDto();
    }
}
