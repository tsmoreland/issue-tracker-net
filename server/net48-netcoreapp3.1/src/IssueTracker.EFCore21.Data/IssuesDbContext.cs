using System;
using System.Reflection;
using IssueTracker.Core.Model;
using IssueTracker.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace IssueTracker.EFCore21.Data;

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
    public DbSet<Issue> Issues { get; private set; } = null!;

    /// <summary>
    /// Linked Issues data set
    /// </summary>
    public DbSet<LinkedIssue> LinkedIssues { get; private set; } = null!;

    public static void Configure(DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("ApplicationConnection");
        optionsBuilder
            .UseSqlite(
                connectionString,
                options => options.MigrationsAssembly(typeof(IssuesDbContext).Assembly.FullName));
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        Configure(optionsBuilder, _configuration);
        base.OnConfiguring(optionsBuilder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        EntityTypeBuilder<Issue> issueEntity = modelBuilder.Entity<Issue>();
        issueEntity.ToTable("Issues").HasKey(e => e.Id);
        issueEntity.HasIndex(e => e.Title);
        issueEntity.HasIndex(e => e.Priority);

        issueEntity.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
        issueEntity.Property(e => e.Title).IsRequired().IsUnicode().HasMaxLength(200);
        issueEntity.Property(e => e.Description).IsUnicode().HasMaxLength(500);
        issueEntity.Property(e => e.Priority).IsRequired();
        issueEntity.HasMany(typeof(LinkedIssue), "ChildIssueEntities").WithOne("ChildIssue");
        issueEntity.HasMany(typeof(LinkedIssue), "ParentIssueEntities").WithOne("ParentIssue");
        issueEntity.Property(e => e.ConcurrencyToken).IsConcurrencyToken();
        issueEntity.Property(e => e.LastUpdated)
            .HasConversion(dateTime => dateTime.Ticks, ticks => new DateTime(ticks))
            .ValueGeneratedOnAddOrUpdate()
            .HasValueGenerator<DateTimeValueGenerator>();

        // without these we'd get foregin key warnings becuase we're using ParentIssueEntities and ChildIssueEntites as the data backing (private properties)
        issueEntity.Ignore(e => e.ParentIssues);
        issueEntity.Ignore(e => e.ChildIssues);

        EntityTypeBuilder<LinkedIssue> linkedIssueEntity = modelBuilder.Entity<LinkedIssue>();
        linkedIssueEntity.ToTable("LinkedIssues").HasKey(e => e.Id);
        linkedIssueEntity.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
        linkedIssueEntity.HasOne(e => e.ParentIssue).WithMany("ParentIssueEntities").HasForeignKey(e => e.ParentIssueId);
        linkedIssueEntity.HasOne(e => e.ChildIssue).WithMany("ChildIssueEntities").HasForeignKey(e => e.ChildIssueId);
        linkedIssueEntity.HasIndex(e => e.ParentIssueId);
        linkedIssueEntity.HasIndex(e => e.ChildIssueId);

        Seed(modelBuilder);
    }

    private static void Seed(ModelBuilder modelBuilder)
    {
        EntityTypeBuilder<Issue> issueEntity = modelBuilder.Entity<Issue>();

        Issue first = new(1, "First", "First issue",
            Priority.Medium, IssueType.Epic,
            User.Unassigned, User.Unassigned,
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>(),
            new DateTime(2022, 01, 01, 0, 0, 0, DateTimeKind.Utc), Guid.NewGuid().ToString());
        Issue second = new(2, "Second", "Second issue",
            Priority.Low, IssueType.Story,
            User.Unassigned, User.Unassigned,
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>(),
            new DateTime(2022, 01, 020, 0, 0, 0, DateTimeKind.Utc), Guid.NewGuid().ToString());
        Issue third = new (3, "Third", "Third issue",
            Priority.Medium, IssueType.Story,
            User.Unassigned, User.Unassigned,
            Array.Empty<LinkedIssue>(), Array.Empty<LinkedIssue>(),
            new DateTime(2022, 01, 020, 0, 0, 0, DateTimeKind.Utc), Guid.NewGuid().ToString());

        PropertyInfo assigneeProperty = typeof(Issue).GetProperty(nameof(Issue.Assignee));
        if (assigneeProperty is null)
        {
            throw new InvalidOperationException("Unable to locate Assignee property");
        }
        assigneeProperty.SetValue(first, null);
        assigneeProperty.SetValue(second, null);
        assigneeProperty.SetValue(third, null);

        PropertyInfo reporterProperty = typeof(Issue).GetProperty(nameof(Issue.Reporter));
        if (reporterProperty is null)
        {
            throw new InvalidOperationException("Unable to locate Reporter property");
        }
        reporterProperty.SetValue(first, null);
        reporterProperty.SetValue(second, null);
        reporterProperty.SetValue(third, null);

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

        LinkedIssue secondToThird = new(1, LinkType.Related, 2, 3);
        linkedIssuesEntity.HasData(secondToThird);
    }


}
