﻿//
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

using IssueTracker.Issues.Domain.DataContracts;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Commands;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Issues.Infrastructure;

public sealed class IssueDataMigration : IIssueDataMigration
{
    private readonly IssuesDbContext _dbContext;
    private readonly IIssueRepository _repository;

    public IssueDataMigration(IssuesDbContext dbContext, IIssueRepository repository)
    {
        _dbContext = dbContext;
        _repository = repository;
    }

    /// <inheritdoc />
    public ValueTask MigrateAsync(CancellationToken cancellationToken = default)
    {
        return new ValueTask(_dbContext.Database.MigrateAsync(cancellationToken));
    }

    /// <inheritdoc />
    public async ValueTask ResetAndRepopultateAsync(CancellationToken cancellationToken = default)
    {
        // it's playing up again, we can re-enable the delete *if* we ensure the file exists after (at 0 bytes)
        await _dbContext.Database.EnsureDeletedAsync(cancellationToken);

        byte[] content = Array.Empty<byte>();
        await File.WriteAllBytesAsync("issueTracker.db", content, cancellationToken);
        await _dbContext.Database.MigrateAsync(cancellationToken);

        await Seed(cancellationToken);
    }

    private async ValueTask Seed(CancellationToken cancellationToken)
    {
        Project app = new("APP", "Sample Project");
        _dbContext.Projects.Add(app);

        Issue epic = new(app, 1,
            "Add Issue Context",
            "implement issue context with id-based references to project domain",
            Priority.High, IssueType.Epic);
        Issue story = new(app, 2,
            "Add Issue Domain Layer",
            "implement issue domain layer",
            Priority.High, IssueType.Story)
        {
            EpicId = new IssueIdentifier(epic.Id.Project, epic.Id.IssueNumber),
        };

        _repository.Add(epic);
        _repository.Add(story);

        Issue task = new(app, 3,
            "Add commands to modify state",
            "add support for modifying issue state",
            Priority.Medium, IssueType.Task)
        {
            EpicId = new IssueIdentifier(epic.Id.Project, epic.Id.IssueNumber),
        };
        task.Execute(new OpenStateChangeCommand(task.Id, DateTimeOffset.UtcNow));

        Issue linkedIssueTask1 = new(app, 4,
            "verify related to",
            "add issue with related link to verify database adds it",
            Priority.Medium, IssueType.Task)
        {
            EpicId = new IssueIdentifier(epic.Id.Project, epic.Id.IssueNumber),
        };
        linkedIssueTask1.Execute(new OpenStateChangeCommand(linkedIssueTask1.Id, DateTimeOffset.UtcNow));
        linkedIssueTask1.Execute(new ReadyForReviewStateChangeCommand(linkedIssueTask1.Id));

        Issue linkedIssueTask2 = new(app, 5,
            "verify related from",
            "add issue with related link to verify database adds it",
            Priority.Medium, IssueType.Task)
        {
            EpicId = new IssueIdentifier(epic.Id.Project, epic.Id.IssueNumber),
        };

        linkedIssueTask2.Execute(new OpenStateChangeCommand(linkedIssueTask2.Id, DateTimeOffset.UtcNow));
        linkedIssueTask2.Execute(new ReadyForReviewStateChangeCommand(linkedIssueTask2.Id));
        linkedIssueTask2.Execute(new ReadyForTestStateChangeCommand(linkedIssueTask2.Id));
        linkedIssueTask1.AddLinkToChild(LinkType.Related, linkedIssueTask2);

        _repository.Add(task);
        _repository.Add(linkedIssueTask1);
        _repository.Add(linkedIssueTask2);

        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
