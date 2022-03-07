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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace IssueTracker.Data;

/// <summary>
/// Issue Tracker Application Database context
/// </summary>
public sealed class IssuesDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Instantiates a new instance of the <see cref="IssuesDbContext"/> class.
    /// </summary>
    public IssuesDbContext(DbContextOptions<IssuesDbContext> dbContextOptions, IConfiguration configuration)
        : base(dbContextOptions)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Issues data set
    /// </summary>
    public DbSet<Issue> Issues { get; init; } = null!;

    /// <summary>
    /// Linked Issues data set
    /// </summary>
    public DbSet<LinkedIssue> LinkedIssues { get; init; } = null!;

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        string connectionString = _configuration.GetConnectionString("ApplicationConnection");
        optionsBuilder
            .UseSqlite(
                connectionString,
                options => options.MigrationsAssembly(typeof(IssuesDbContext).Assembly.FullName));

        base.OnConfiguring(optionsBuilder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        EntityTypeBuilder<Issue> issueEntity = modelBuilder.Entity<Issue>();
        issueEntity.ToTable("Issues").HasKey(e => e.Id);
        issueEntity.HasIndex(e => e.Title);
        issueEntity.HasIndex(e => e.Priority);

        issueEntity.Property(e => e.Id).IsRequired();
        issueEntity.Property(e => e.Title).IsRequired().IsUnicode().HasMaxLength(200);
        issueEntity.Property(e => e.Description).IsUnicode().HasMaxLength(500);
        issueEntity.Property(e => e.Priority).IsRequired();
        issueEntity.Property(e => e.ConcurrencyToken).IsConcurrencyToken();
        issueEntity.HasMany("ChildIssueEntities").WithOne();
        issueEntity.HasMany("ParentIssueEntities").WithOne();

        // without these we'd get foregin key warnings becuase we're using ParentIssueEntities and ChildIssueEntites as the data backing (private properties)
        issueEntity.Ignore(e => e.ParentIssues);
        issueEntity.Ignore(e => e.ChildIssues);

        Issue first = new(new Guid("1385056E-8AFA-4E09-96DF-AE12EFDF1A29"), "First", "First issue", Priority.Medium,
            new DateTime(2022, 01, 01, 0, 0, 0, DateTimeKind.Utc), Guid.NewGuid().ToString(),
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>());
        Issue second = new (new Guid("A28B8C45-6668-4169-940C-C16D71EB69DE"), "Second", "Second issue",
            Priority.Low, new DateTime(2022, 01, 020, 0, 0, 0, DateTimeKind.Utc), Guid.NewGuid().ToString(),
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>());
        Issue third = new (new Guid("502AD68E-7B37-4426-B422-23B6A9B1B7CA"), "Third", "Third issue",
            Priority.Medium, new DateTime(2022, 01, 020, 0, 0, 0, DateTimeKind.Utc), Guid.NewGuid().ToString(),
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>());
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

        LinkedIssue secondToThird = new(LinkType.Related, new Guid("A28B8C45-6668-4169-940C-C16D71EB69DE"), new Guid("502AD68E-7B37-4426-B422-23B6A9B1B7CA"));
        linkedIssuesEntity.HasData(secondToThird);

    }
}
