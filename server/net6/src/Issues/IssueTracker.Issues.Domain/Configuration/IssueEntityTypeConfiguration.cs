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

using IssueTracker.Issues.Domain.Configuration.ValueConverters;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssueTracker.Issues.Domain.Configuration;

internal sealed class IssueEntityTypeConfiguration : IEntityTypeConfiguration<Issue>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion<IssueIdentifierValueConverter>()
            .IsRequired();
        builder.Property<string>("_projectId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasMaxLength(3)
            .IsUnicode(false)
            .IsRequired();
        builder.Property<int>("_issueNumber")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired();
        builder.Property<string>("_title")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired().IsUnicode().HasMaxLength(200);
        builder.Property<string>("_description")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsUnicode()
            .HasMaxLength(500);
        builder
            .Property(e => e.Priority)
            .IsRequired();
        builder
            .Property<IssueType>("_type")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired();

        builder
            .Property<DateTimeOffset?>("_startTime")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasConversion<NullableDateTimeOffsetValueConverter>();

        builder
            .Property<DateTimeOffset?>("_stopTime")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasConversion<NullableDateTimeOffsetValueConverter>();

        builder
            .Property(e => e.State)
            .IsRequired()
            .HasConversion<IssueStateValueConverter>();

        builder.Property<IssueIdentifier?>("_epicId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasConversion<NullableIssueIdentifierValueConverter>()
            .IsRequired(false);

        builder
            .HasOne<Issue>()
            .WithMany()
            .HasForeignKey("_epicId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder.OwnsOne(e => e.Assignee,
            (owned) =>
            {
                owned
                    .Property(e => e.UserId)
                    .IsRequired();
                owned
                    .Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(true);
            });
        builder.OwnsOne(e => e.Reporter,
            (owned) =>
            {
                owned
                    .Property(e => e.UserId)
                    .IsRequired();
                owned
                    .Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(true);
            });

        builder
            .HasMany("_relatedTo")
            .WithOne();
        builder
            .HasMany("_relatedFrom")
            .WithOne();
    }
}
