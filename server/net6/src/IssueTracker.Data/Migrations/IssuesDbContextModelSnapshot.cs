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
                    b.Property<Guid>("IssueId")
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

                    b.Property<Guid?>("EpicIssueId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("IssueNumber")
                        .HasColumnType("INTEGER");

                    b.Property<long>("LastUpdated")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Priority")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Project")
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

                    b.HasKey("IssueId");

                    b.HasIndex("EpicIssueId");

                    b.HasIndex("Id");

                    b.HasIndex("IssueNumber");

                    b.HasIndex("Priority");

                    b.HasIndex("Project");

                    b.HasIndex("Title");

                    b.ToTable("Issues", (string)null);

                    b.HasData(
                        new
                        {
                            IssueId = new Guid("ee88fef9-ccf4-41b0-8326-1200f60b308b"),
                            ConcurrencyToken = "a8bd5368-4ce4-4d60-98a5-e89e0c15d855",
                            Description = "add projects and use project id as link between issue and project",
                            Id = "APP-1",
                            IssueNumber = 1,
                            LastUpdated = 637766370000000000L,
                            Priority = 2,
                            Project = "APP",
                            Title = "Add Project support",
                            Type = 0
                        },
                        new
                        {
                            IssueId = new Guid("33e3d8bf-af55-498a-b96f-f514a3b0a39d"),
                            ConcurrencyToken = "c5cbc3aa-0a9e-4ccb-96de-b4bf61854d5b",
                            Description = "",
                            Id = "APP-2",
                            IssueNumber = 2,
                            LastUpdated = 637766376000000000L,
                            Priority = 1,
                            Project = "APP",
                            Title = "The database should story issues with links to projects",
                            Type = 1
                        },
                        new
                        {
                            IssueId = new Guid("cd9a0df2-5f83-4942-9c08-4d97b2a90d55"),
                            ConcurrencyToken = "1c0b0db4-1a94-4af7-903e-9bbf8fc71ce6",
                            Description = "As a user I want to be able to retreive all projects",
                            Id = "APP-3",
                            IssueNumber = 3,
                            LastUpdated = 637766379000000000L,
                            Priority = 0,
                            Project = "APP",
                            Title = "The api should be able to retrieve projects",
                            Type = 1
                        },
                        new
                        {
                            IssueId = new Guid("794c169c-a1dd-45ee-b093-fd80c25a28bf"),
                            ConcurrencyToken = "132e3b89-ce8d-4ed7-9a6b-81410c11832e",
                            Description = "add the model(s) for project type ensuring it's id matches the expectations of issue",
                            Id = "APP-4",
                            IssueNumber = 4,
                            LastUpdated = 637767216000000000L,
                            Priority = 1,
                            Project = "APP",
                            Title = "add project core models",
                            Type = 2
                        },
                        new
                        {
                            IssueId = new Guid("b0454fc6-9014-4dc8-bccc-a90b997e7b02"),
                            ConcurrencyToken = "4a9f3a8b-5618-4daf-b6db-a254857f5287",
                            Description = "add the request handlers to get projects by id and a summary",
                            Id = "APP-5",
                            IssueNumber = 5,
                            LastUpdated = 637768143000000000L,
                            Priority = 0,
                            Project = "APP",
                            Title = "add mediator request/handlers for project query",
                            Type = 2
                        });
                });

            modelBuilder.Entity("IssueTracker.Core.ValueObjects.LinkedIssue", b =>
                {
                    b.Property<Guid>("ParentIssueId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ChildIssueId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChildId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<int>("LinkType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ParentId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ParentIssueId", "ChildIssueId");

                    b.HasIndex("ChildId");

                    b.HasIndex("ChildIssueId");

                    b.HasIndex("ParentId");

                    b.ToTable("LinkedIssues", (string)null);

                    b.HasData(
                        new
                        {
                            ParentIssueId = new Guid("794c169c-a1dd-45ee-b093-fd80c25a28bf"),
                            ChildIssueId = new Guid("b0454fc6-9014-4dc8-bccc-a90b997e7b02"),
                            ChildId = "APP-5",
                            ConcurrencyToken = "69925489-fdbf-4645-a0a7-928316b6d3b7",
                            LinkType = 2,
                            ParentId = "APP-4"
                        },
                        new
                        {
                            ParentIssueId = new Guid("33e3d8bf-af55-498a-b96f-f514a3b0a39d"),
                            ChildIssueId = new Guid("cd9a0df2-5f83-4942-9c08-4d97b2a90d55"),
                            ChildId = "APP-3",
                            ConcurrencyToken = "6082d1e3-d7a5-413d-873e-9bca4715d346",
                            LinkType = 0,
                            ParentId = "APP-2"
                        });
                });

            modelBuilder.Entity("IssueTracker.Core.Model.Issue", b =>
                {
                    b.HasOne("IssueTracker.Core.Model.Issue", null)
                        .WithMany()
                        .HasForeignKey("EpicIssueId");

                    b.OwnsOne("IssueTracker.Core.ValueObjects.Maintainer", "Assignee", b1 =>
                        {
                            b1.Property<Guid>("IssueId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("FullName")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(200)
                                .IsUnicode(true)
                                .HasColumnType("TEXT")
                                .HasDefaultValue("Unassigned");

                            b1.Property<Guid>("UserId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("TEXT")
                                .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                            b1.HasKey("IssueId");

                            b1.ToTable("Issues");

                            b1.WithOwner()
                                .HasForeignKey("IssueId");
                        });

                    b.OwnsOne("IssueTracker.Core.ValueObjects.TriageUser", "Reporter", b1 =>
                        {
                            b1.Property<Guid>("IssueId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("FullName")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(200)
                                .IsUnicode(true)
                                .HasColumnType("TEXT")
                                .HasDefaultValue("Unassigned");

                            b1.Property<Guid>("UserId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("TEXT")
                                .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

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
