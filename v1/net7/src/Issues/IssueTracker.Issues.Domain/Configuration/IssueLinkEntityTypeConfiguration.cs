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

using IssueTracker.Issues.Domain.Configuration.ValueConverters;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssueTracker.Issues.Domain.Configuration;

internal sealed class IssueLinkEntityTypeConfiguration : IEntityTypeConfiguration<IssueLink>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<IssueLink> builder)
    {
        builder.HasKey(e => new { e.ParentId, e.ChildId });

        builder
            .Property(e => e.ChildId)
            .IsRequired();

        builder
            .Property(e => e.ParentId)
            .HasConversion<IssueIdentifierValueConverter>()
            .IsRequired();
        builder
            .Property(e => e.ChildId)
            .HasConversion<IssueIdentifierValueConverter>()
            .IsRequired();

        builder
            .HasOne(e => e.Parent)
            .WithMany("_children")
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.ChildId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(e => e.Child)
            .WithMany("_parents")
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
