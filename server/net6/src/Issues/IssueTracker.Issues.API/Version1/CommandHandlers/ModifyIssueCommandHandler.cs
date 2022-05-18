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

using IssueTracker.Issues.API.Version1.Abstractions.Commands;
using IssueTracker.Issues.API.Version1.Abstractions.DataTransferObjects;
using IssueTracker.Issues.API.Version1.Extensions;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using MediatR;

namespace IssueTracker.Issues.API.Version1.CommandHandlers;

public sealed class ModifyIssueCommandHandler : IRequestHandler<ModifyIssueCommand, IssueDto?>
{
    private readonly IIssueRepository _repository;

    public ModifyIssueCommandHandler(IIssueRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    /// <inheritdoc />
    public async Task<IssueDto?> Handle(ModifyIssueCommand request, CancellationToken cancellationToken)
    {
        (IssueIdentifier id, string? title, string? description, Priority? priority) = request;

        Issue? issue = await _repository.GetByIdOrDefault(id, true, cancellationToken);
        if (issue is null)
        {
            return null;
        }

        if (title is not null)
        {
            issue.Title = title;
        }

        if (description is not null)
        {
            issue.Description = description;
        }

        if (priority is not null)
        {
            issue.Priority = priority.Value;
        }

        issue = _repository.Update(issue);

        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return issue.ToDto();
    }
}
