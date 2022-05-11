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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssueTracker.Issues.Infrastructure.Configurations;

internal sealed class IssueEntityTypeConfiguration : IEntityTypeConfiguration<Issue>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder
            .ToTable("Issues")
            .HasKey(e => e.Id);

        builder.HasIndex(e => e.Title);
        builder.HasIndex(e => e.Priority);
        builder.HasIndex("_project");
        builder.HasIndex("_issueNumber");

        builder.Property(e => e.Id)
            .HasConversion(e => e.ToString(), @string => IssueIdentifier.FromString(@string))
            .IsRequired();
        builder.Ignore(e => e.Project);
        builder.Property<string>("_project")
            .HasColumnName("Project")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasMaxLength(3)
            .IsUnicode(false)
            .IsRequired();
        builder.Ignore(e => e.IssueNumber);
        builder.Property<int>("_issueNumber")
            .HasColumnName("IssueNumber")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired();
        builder.Property(e => e.Title).IsRequired().IsUnicode().HasMaxLength(200);
        builder.Property(e => e.Description).IsUnicode().HasMaxLength(500);
        builder.Ignore(e => e.Priority);
        builder
            .Property(e => e.Priority)
            .IsRequired();
        builder.Ignore(e => e.Type);
        builder
            .Property<IssueType>("_type")
            .HasColumnName("Type")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired();

        builder.Ignore(e => e.EpicId);
        builder.Property<IssueIdentifier?>("_epicId")
            .HasColumnName("EpicId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasConversion(e => e != null ? e.ToString() : null!,
                @string => !string.IsNullOrEmpty(@string) ? IssueIdentifier.FromString(@string) : null)
            .IsRequired(false);

        builder
            .HasOne<Issue>()
            .WithMany()
            .HasForeignKey("_epicId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Property(e => e.ConcurrencyToken).IsConcurrencyToken();
        builder.Property(e => e.LastModifiedTime)
            .HasConversion(
                dateTime => dateTime.ToUniversalTime().Ticks,
                ticks => new DateTimeOffset(new DateTime(ticks, DateTimeKind.Utc), TimeSpan.FromSeconds(0)));

        builder.OwnsOne(e => e.Assignee,
            (owned) =>
            {
                owned
                    .Property(e => e.UserId)
                    .IsRequired()
                    .HasDefaultValue(TriageUser.Unassigned.UserId);
                owned
                    .Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(true)
                    .HasDefaultValue(TriageUser.Unassigned.FullName);
            });
        builder.OwnsOne(e => e.Reporter,
            (owned) =>
            {
                owned
                    .Property(e => e.UserId)
                    .IsRequired()
                    .HasDefaultValue(Maintainer.Unassigned.UserId);
                owned
                    .Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(true)
                    .HasDefaultValue(Maintainer.Unassigned.FullName);
            });

        builder.Ignore(e => e.RelatedIssues);
        builder
            .HasMany("_relatedTo")
            .WithOne();
        builder
            .HasMany("_relatedFrom")
            .WithOne();
    }
}

internal sealed class IssueLinkEntityTypeConfiguration : IEntityTypeConfiguration<IssueLink>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<IssueLink> builder)
    {
        builder.ToTable("IssueLinks").HasKey(e => new { e.LeftId, e.RightId });

        builder
            .Property(e => e.Link)
            .IsRequired()
            .HasDefaultValue(LinkType.Related);

        builder
            .Property(e => e.LeftId)
            .HasConversion(e => e.ToString(), @string => IssueIdentifier.FromString(@string))
            .IsRequired();
        builder
            .Property(e => e.RightId)
            .HasConversion(e => e.ToString(), @string => IssueIdentifier.FromString(@string))
            .IsRequired();

        builder
            .HasOne(e => e.Left)
            .WithMany("_relatedFrom")
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.LeftId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(e => e.Right)
            .WithMany("_relatedTo")
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.RightId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

