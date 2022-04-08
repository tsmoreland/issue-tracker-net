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

using System;
using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Core.Model;
using IssueTracker.Data.Abstractions;

namespace IssueTracker.Data;

public sealed class IssueDataMigration : IIssueDataMigration
{
    private readonly IIssueRepository _repository;

    public IssueDataMigration(IIssueRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc />
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        Issue first = new(new Guid("1385056E-8AFA-4E09-96DF-AE12EFDF1A29"), "First", "First issue",
            Priority.Medium, IssueType.Epic,
            new DateTime(2022, 01, 01, 0, 0, 0, DateTimeKind.Utc), Guid.NewGuid().ToString(),
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>());
        Issue second = new (new Guid("A28B8C45-6668-4169-940C-C16D71EB69DE"), "Second", "Second issue",
            Priority.Low, IssueType.Story,
            new DateTime(2022, 01, 020, 0, 0, 0, DateTimeKind.Utc), Guid.NewGuid().ToString(),
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>());
        Issue third = new (new Guid("502AD68E-7B37-4426-B422-23B6A9B1B7CA"), "Third", "Third issue",
            Priority.Medium, IssueType.Story,
            new DateTime(2022, 01, 020, 0, 0, 0, DateTimeKind.Utc), Guid.NewGuid().ToString(),
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>());

        await Task.WhenAll(
            _repository.AddIssue(first, cancellationToken),
            _repository.AddIssue(second, cancellationToken),
            _repository.AddIssue(third, cancellationToken));
    }

    /// <inheritdoc />
    public void Migrate()
    {
        MigrateAsync(CancellationToken.None).Wait();
    }
}
