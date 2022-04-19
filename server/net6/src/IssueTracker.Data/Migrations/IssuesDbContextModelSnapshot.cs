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
            modelBuilder.HasAnnotation("ProductVersion", "6.0.2");

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

                    b.HasData(
                        new
                        {
                            Id = new Guid("1385056e-8afa-4e09-96df-ae12efdf1a29"),
                            ConcurrencyToken = "d9515a75-c02e-43d2-a47e-3e8be987924b",
                            Description = "First issue",
                            LastUpdated = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Priority = 1,
                            Title = "First",
                            Type = 0
                        },
                        new
                        {
                            Id = new Guid("a28b8c45-6668-4169-940c-c16d71eb69de"),
                            ConcurrencyToken = "b9998e94-8a08-44e1-bc73-6e479fadee90",
                            Description = "Second issue",
                            LastUpdated = new DateTime(2022, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                            Priority = 0,
                            Title = "Second",
                            Type = 1
                        },
                        new
                        {
                            Id = new Guid("502ad68e-7b37-4426-b422-23b6a9b1b7ca"),
                            ConcurrencyToken = "c126a9bc-2ad5-4e33-a59e-e665c0c6dc35",
                            Description = "Third issue",
                            LastUpdated = new DateTime(2022, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                            Priority = 1,
                            Title = "Third",
                            Type = 1
                        });
                });

            modelBuilder.Entity("IssueTracker.Core.Model.LinkedIssue", b =>
                {
                    b.Property<Guid>("ParentIssueId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ChildIssueId")
                        .HasColumnType("TEXT");

                    b.Property<int>("LinkType")
                        .HasColumnType("INTEGER");

                    b.HasKey("ParentIssueId", "ChildIssueId");

                    b.HasIndex("ChildIssueId");

                    b.ToTable("LinkedIssue", (string)null);

                    b.HasData(
                        new
                        {
                            ParentIssueId = new Guid("a28b8c45-6668-4169-940c-c16d71eb69de"),
                            ChildIssueId = new Guid("502ad68e-7b37-4426-b422-23b6a9b1b7ca"),
                            LinkType = 0
                        });
                });

            modelBuilder.Entity("IssueTracker.Core.Model.LinkedIssue", b =>
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
