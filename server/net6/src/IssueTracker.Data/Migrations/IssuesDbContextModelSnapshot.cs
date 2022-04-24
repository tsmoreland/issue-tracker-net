﻿// <auto-generated />
using System;
using IssueTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IssueTracker.Data.Migrations
{
    [DbContext(typeof(IssuesDbContext))]
    partial class IssuesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.4");

            modelBuilder.Entity("IssueTracker.Core.Model.Issue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(true)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<int>("Priority")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(true)
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Priority");

                    b.HasIndex("Title");

                    b.ToTable("Issues", (string)null);
                });

            modelBuilder.Entity("IssueTracker.Core.Model.LinkedIssue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ChildIssueId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyToken")
                        .HasColumnType("TEXT");

                    b.Property<int>("LinkType")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ParentIssueId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChildIssueId");

                    b.HasIndex("ParentIssueId");

                    b.ToTable("LinkedIssues", (string)null);
                });

            modelBuilder.Entity("IssueTracker.Core.Model.Issue", b =>
                {
                    b.OwnsOne("IssueTracker.Core.ValueObjects.User", "Assignee", b1 =>
                        {
                            b1.Property<Guid>("IssueId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("FullName")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(200)
                                .IsUnicode(true)
                                .HasColumnType("TEXT")
                                .HasDefaultValue("Unassigned")
                                .HasColumnName("AssigneeFullName");

                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("TEXT")
                                .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"))
                                .HasColumnName("AssigneeId");

                            b1.HasKey("IssueId");

                            b1.ToTable("Issues");

                            b1.WithOwner()
                                .HasForeignKey("IssueId");
                        });

                    b.OwnsOne("IssueTracker.Core.ValueObjects.User", "Reporter", b1 =>
                        {
                            b1.Property<Guid>("IssueId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("FullName")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(200)
                                .IsUnicode(true)
                                .HasColumnType("TEXT")
                                .HasDefaultValue("Unassigned")
                                .HasColumnName("ReporterFullName");

                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("TEXT")
                                .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"))
                                .HasColumnName("ReporterId");

                            b1.HasKey("IssueId");

                            b1.ToTable("Issues");

                            b1.WithOwner()
                                .HasForeignKey("IssueId");
                        });

                    b.Navigation("Assignee")
                        .IsRequired();

                    b.Navigation("Reporter")
                        .IsRequired();
                });

            modelBuilder.Entity("IssueTracker.Core.Model.LinkedIssue", b =>
                {
                    b.HasOne("IssueTracker.Core.Model.Issue", "ChildIssue")
                        .WithMany("ChildIssueEntities")
                        .HasForeignKey("ChildIssueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IssueTracker.Core.Model.Issue", "ParentIssue")
                        .WithMany("ParentIssueEntities")
                        .HasForeignKey("ParentIssueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChildIssue");

                    b.Navigation("ParentIssue");
                });

            modelBuilder.Entity("IssueTracker.Core.Model.Issue", b =>
                {
                    b.Navigation("ChildIssueEntities");

                    b.Navigation("ParentIssueEntities");
                });
#pragma warning restore 612, 618
        }
    }
}
