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

using IssueTracker.Issues.Infrastructure.Configurations.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssueTracker.Issues.Infrastructure.Configurations;

internal sealed class IssueEntityTypeConfiguration : IEntityTypeConfiguration<Issue>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder
            .ToTable("Issues");

        builder.HasIndex(e => e.Priority);
        builder.HasIndex("_title");
        builder.HasIndex("_project");
        builder.HasIndex("_issueNumber");

        builder.Ignore(e => e.Project);
        builder.Ignore(e => e.IssueNumber);
        builder.Ignore(e => e.Title);
        builder.Ignore(e => e.Description);
        builder.Ignore(e => e.Type);
        builder.Ignore(e => e.EpicId);
        builder.Ignore(e => e.StartTime);
        builder.Ignore(e => e.StopTime);

        builder.Property<string>("_project")
            .HasColumnName("Project");
        builder.Property<int>("_issueNumber")
            .HasColumnName("IssueNumber");
        builder.Property<string>("_title")
            .HasColumnName("Title");
        builder.Property<string>("_description")
            .HasColumnName("Description");
        builder.Property<IssueType>("_type")
            .HasColumnName("Type");

        builder.Property<IssueIdentifier?>("_epicId")
            .HasColumnName("EpicId");

        builder.Property(e => e.ConcurrencyToken).IsConcurrencyToken();
        builder.Property(e => e.LastModifiedTime)
            .HasConversion<DateTimeOffsetValueConverter>();

        builder.OwnsOne(e => e.Assignee,
            (owned) =>
            {
                owned.Property(e => e.UserId)
                    .HasDefaultValue(TriageUser.Unassigned.UserId);
                owned.Property(e => e.FullName)
                    .HasDefaultValue(TriageUser.Unassigned.FullName);
            });
        builder.OwnsOne(e => e.Reporter,
            (owned) =>
            {
                owned.Property(e => e.UserId)
                    .HasDefaultValue(Maintainer.Unassigned.UserId);
                owned.Property(e => e.FullName)
                    .HasDefaultValue(Maintainer.Unassigned.FullName);
            });

        builder.Ignore(e => e.RelatedIssues);
    }
}
