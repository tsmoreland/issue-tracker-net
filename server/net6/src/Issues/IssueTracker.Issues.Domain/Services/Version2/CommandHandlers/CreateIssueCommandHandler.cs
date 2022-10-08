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
using IssueTracker.Issues.Domain.Services.Version2.Commands;
using IssueTracker.Issues.Domain.Services.Version2.DataTransferObjects;
using IssueTracker.Issues.Domain.Services.Version2.Extensions;
using MediatR;

namespace IssueTracker.Issues.Domain.Services.Version2.CommandHandlers;

public sealed class CreateIssueCommandHandler : IRequestHandler<CreateIssueCommand, IssueDto>
{
    private readonly IIssueRepository _repository;

    public CreateIssueCommandHandler(IIssueRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc />
    public async Task<IssueDto> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
    {

        (string projectId, string title, string description, Priority priority, IssueType type, IssueIdentifier? epicId) = request;
        int issueNumber = await _repository.MaxIssueNumber(projectId, cancellationToken) + 1;

        Project? project = await _repository.FindProjectById(projectId, cancellationToken);
        if (project is null)
        {
            throw new ArgumentException("Project not found", nameof(request));
        }

        Issue issue = project.CreateIssue()
            .WithIssueNumber(issueNumber)
            .WithTitle(title)
            .WithDescription(description)
            .WithPriority(priority)
            .WithType(type)
            .WithEpic(epicId)
            .Build();

        issue = _repository.Add(issue);

        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return issue.ToDto();
    }
}
