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

using IssueTracker.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Data;

public sealed class IssueDataMigration : IIssueDataMigration
{
    private readonly IssuesDbContext _dbContext;

    public IssueDataMigration(IssuesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public ValueTask MigrateAsync(CancellationToken cancellationToken)
    {
        return new ValueTask(_dbContext.Database.MigrateAsync(cancellationToken));
    }

    /// <inheritdoc />
    public void Migrate()
    {
        _dbContext.Database.Migrate();
    }

    /// <inheritdoc />
    public ValueTask SeedAync(IIssueRepository repository, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }


    /* for reference:
    private static void Seed(ModelBuilder modelBuilder)
    {
        EntityTypeBuilder<Issue> issueEntity = modelBuilder.Entity<Issue>();
        Issue first = new(new Guid("1385056E-8AFA-4E09-96DF-AE12EFDF1A29"), "First", "First issue",
            Priority.Medium, IssueType.Epic,
            new DateTime(2022, 01, 01, 0, 0, 0, DateTimeKind.Utc), Guid.NewGuid().ToString(),
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>(), User.Unassigned, User.Unassigned);
        Issue second = new (new Guid("A28B8C45-6668-4169-940C-C16D71EB69DE"), "Second", "Second issue",
            Priority.Low, IssueType.Story,
            new DateTime(2022, 01, 020, 0, 0, 0, DateTimeKind.Utc), Guid.NewGuid().ToString(),
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>(), User.Unassigned, User.Unassigned);
        Issue third = new (new Guid("502AD68E-7B37-4426-B422-23B6A9B1B7CA"), "Third", "Third issue",
            Priority.Medium, IssueType.Story,
            new DateTime(2022, 01, 020, 0, 0, 0, DateTimeKind.Utc), Guid.NewGuid().ToString(),
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>(), User.Unassigned, User.Unassigned);
        issueEntity.HasData(first, second, third);

        EntityTypeBuilder<LinkedIssue> linkedIssuesEntity = modelBuilder.Entity<LinkedIssue>();
        linkedIssuesEntity.ToTable("LinkedIssue")
            .HasKey(e => new { e.ParentIssueId, e.ChildIssueId });
        linkedIssuesEntity
            .HasOne(e => e.ParentIssue)
            .WithMany("ParentIssueEntities")
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.ParentIssueId)
            .OnDelete(DeleteBehavior.Restrict);
        linkedIssuesEntity
            .HasOne(e => e.ChildIssue)
            .WithMany("ChildIssueEntities")
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.ChildIssueId)
            .OnDelete(DeleteBehavior.Restrict);

        LinkedIssue secondToThird = new(
            Guid.NewGuid(),
            LinkType.Related,
            new Guid("A28B8C45-6668-4169-940C-C16D71EB69DE"),
            new Guid("502AD68E-7B37-4426-B422-23B6A9B1B7CA"),
            Guid.NewGuid().ToString());
        linkedIssuesEntity.HasData(secondToThird);
    }
    */
}
