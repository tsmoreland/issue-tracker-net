﻿// <auto-generated />
using System;
using IssueTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IssueTracker.Data.Migrations
{
    [DbContext(typeof(IssuesDbContext))]
    [Migration("20220501135411_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.4");

            modelBuilder.Entity("IssueTracker.Core.Model.Issue", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(true)
                        .HasColumnType("TEXT");

                    b.Property<int>("IssueNumber")
                        .HasColumnType("INTEGER");

                    b.Property<long>("LastUpdated")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Priority")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasMaxLength(3)
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(true)
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("IssueNumber");

                    b.HasIndex("Priority");

                    b.HasIndex("ProjectId");

                    b.HasIndex("Title");

                    b.ToTable("Issues", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "APP-1",
                            ConcurrencyToken = "e653141e-0cf0-4da2-97be-b54e3de00cb3",
                            Description = "add projects and use project id as link between issue and project",
                            IssueNumber = 1,
                            LastUpdated = 637766370000000000L,
                            Priority = 2,
                            ProjectId = "APP",
                            Title = "Add Project support",
                            Type = 0
                        },
                        new
                        {
                            Id = "APP-2",
                            ConcurrencyToken = "a69efa66-c58e-4330-b37d-9796aa76a37d",
                            Description = "",
                            IssueNumber = 2,
                            LastUpdated = 637766376000000000L,
                            Priority = 1,
                            ProjectId = "APP",
                            Title = "The database should story issues with links to projects",
                            Type = 1
                        },
                        new
                        {
                            Id = "APP-3",
                            ConcurrencyToken = "14730f8b-c599-44d0-8a6f-b4d7e9088fa1",
                            Description = "As a user I want to be able to retreive all projects",
                            IssueNumber = 3,
                            LastUpdated = 637766379000000000L,
                            Priority = 0,
                            ProjectId = "APP",
                            Title = "The api should be able to retrieve projects",
                            Type = 1
                        },
                        new
                        {
                            Id = "APP-4",
                            ConcurrencyToken = "4323ddf2-77b5-4cef-b430-7590a05c933c",
                            Description = "add the model(s) for project type ensuring it's id matches the expectations of issue",
                            IssueNumber = 4,
                            LastUpdated = 637767216000000000L,
                            Priority = 1,
                            ProjectId = "APP",
                            Title = "add project core models",
                            Type = 2
                        },
                        new
                        {
                            Id = "APP-5",
                            ConcurrencyToken = "7d55b877-aafb-4a06-ab98-50d7e300d48a",
                            Description = "add the request handlers to get projects by id and a summary",
                            IssueNumber = 5,
                            LastUpdated = 637768143000000000L,
                            Priority = 0,
                            ProjectId = "APP",
                            Title = "add mediator request/handlers for project query",
                            Type = 2
                        });
                });

            modelBuilder.Entity("IssueTracker.Core.ValueObjects.LinkedIssue", b =>
                {
                    b.Property<string>("ParentIssueId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChildIssueId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<int>("LinkType")
                        .HasColumnType("INTEGER");

                    b.HasKey("ParentIssueId", "ChildIssueId");

                    b.HasIndex("ChildIssueId");

                    b.ToTable("LinkedIssues", (string)null);

                    b.HasData(
                        new
                        {
                            ParentIssueId = "APP-4",
                            ChildIssueId = "APP-5",
                            ConcurrencyToken = "2b988dd1-2002-4a25-b5a0-dbbefff57af1",
                            LinkType = 2
                        },
                        new
                        {
                            ParentIssueId = "APP-2",
                            ChildIssueId = "APP-3",
                            ConcurrencyToken = "b2b4646e-4512-4567-8195-23e08821e2df",
                            LinkType = 0
                        });
                });

            modelBuilder.Entity("IssueTracker.Core.Model.Issue", b =>
                {
                    b.OwnsOne("IssueTracker.Core.ValueObjects.User", "Assignee", b1 =>
                        {
                            b1.Property<string>("IssueId")
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
                            b1.Property<string>("IssueId")
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

            modelBuilder.Entity("IssueTracker.Core.ValueObjects.LinkedIssue", b =>
                {
                    b.HasOne("IssueTracker.Core.Model.Issue", "ChildIssue")
                        .WithMany("ChildIssueEntities")
                        .HasForeignKey("ChildIssueId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("IssueTracker.Core.Model.Issue", "ParentIssue")
                        .WithMany("ParentIssueEntities")
                        .HasForeignKey("ParentIssueId")
                        .OnDelete(DeleteBehavior.Restrict)
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
