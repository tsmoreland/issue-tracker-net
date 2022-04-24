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
using IssueTracker.Core.ValueObjects;
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

        issueEntity.OwnsOne(e => e.Assignee,
            (owned) =>
            {
                owned
                    .Property(e => e.Id)
                    .IsRequired()
                    .HasDefaultValue(User.Unassigned.Id)
                    .HasColumnName("AssigneeId");
                owned
                    .Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(true)
                    .HasDefaultValue(User.Unassigned.FullName)
                    .HasColumnName("AssigneeFullName");
            });
        issueEntity.OwnsOne(e => e.Reporter,
            (owned) =>
            {
                owned
                    .Property(e => e.Id)
                    .IsRequired()
                    .HasDefaultValue(User.Unassigned.Id)
                    .HasColumnName("ReporterId");
                owned
                    .Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(true)
                    .HasDefaultValue(User.Unassigned.FullName)
                    .HasColumnName("ReporterFullName");
            });


        issueEntity.HasMany("ChildIssueEntities").WithOne();
        issueEntity.HasMany("ParentIssueEntities").WithOne();

        // without these we'd get foregin key warnings becuase we're using ParentIssueEntities and ChildIssueEntites as the data backing (private properties)
        issueEntity.Ignore(e => e.ParentIssues);
        issueEntity.Ignore(e => e.ChildIssues);

        EntityTypeBuilder<LinkedIssue> linkedIssueEntity = modelBuilder.Entity<LinkedIssue>();
        linkedIssueEntity.ToTable("LinkedIssues").HasKey(e => e.Id);
        linkedIssueEntity.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
        linkedIssueEntity.HasIndex(e => e.ParentIssueId);
        linkedIssueEntity.HasIndex(e => e.ChildIssueId);
        linkedIssueEntity.HasOne(e => e.ParentIssue).WithMany("ParentIssueEntities").HasForeignKey(e => e.ParentIssueId);
        linkedIssueEntity.HasOne(e => e.ChildIssue).WithMany("ChildIssueEntities").HasForeignKey(e => e.ChildIssueId);

        // HasData to seed is not done here due to complexities of handling Owned Types (at time of writing Assignee and Reporter)
        // in order to use HasData to see these values it would need to be done as part of the OwnsOne:
        /*
        issueEntity.OwnsOne(e => e.Reporter)
            .HasData(new
            {
                UserId = "id of the issue entity", // this is the shadow property created by EF to link the objects, User matches the class name
                Id = "id property of the user object",
                FullName = "fullname property from user table"
            });
        */
    }
}
