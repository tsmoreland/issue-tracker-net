﻿// <auto-generated />
using System;
using IssueTracker.Issues.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IssueTracker.Issues.Infrastructure.Migrations
{
    [DbContext(typeof(IssuesDbContext))]
    [Migration("20220516004257_TitleCorrection")]
    partial class TitleCorrection
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.4");

            modelBuilder.Entity("IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Issue", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(true)
                        .HasColumnType("TEXT");

                    b.Property<long>("LastModifiedTime")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Priority")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Title1");

                    b.Property<string>("_epicId")
                        .HasColumnType("TEXT")
                        .HasColumnName("EpicId");

                    b.Property<int>("_issueNumber")
                        .HasColumnType("INTEGER")
                        .HasColumnName("IssueNumber");

                    b.Property<string>("_project")
                        .IsRequired()
                        .HasMaxLength(3)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("Project");

                    b.Property<string>("_title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(true)
                        .HasColumnType("TEXT")
                        .HasColumnName("Title");

                    b.Property<int>("_type")
                        .HasColumnType("INTEGER")
                        .HasColumnName("Type");

                    b.HasKey("Id");

                    b.HasIndex("Title");

                    b.HasIndex("_epicId");

                    b.HasIndex("_issueNumber");

                    b.HasIndex("_project");

                    b.ToTable("Issues", (string)null);
                });

            modelBuilder.Entity("IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.IssueLink", b =>
                {
                    b.Property<string>("LeftId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RightId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Link")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.HasKey("LeftId", "RightId");

                    b.HasIndex("RightId");

                    b.ToTable("IssueLinks", (string)null);
                });

            modelBuilder.Entity("IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Issue", b =>
                {
                    b.HasOne("IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Issue", null)
                        .WithMany()
                        .HasForeignKey("_epicId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.OwnsOne("IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Maintainer", "Assignee", b1 =>
                        {
                            b1.Property<string>("IssueId")
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

                    b.OwnsOne("IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.TriageUser", "Reporter", b1 =>
                        {
                            b1.Property<string>("IssueId")
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

            modelBuilder.Entity("IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.IssueLink", b =>
                {
                    b.HasOne("IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Issue", "Left")
                        .WithMany("_relatedFrom")
                        .HasForeignKey("LeftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Issue", "Right")
                        .WithMany("_relatedTo")
                        .HasForeignKey("RightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Left");

                    b.Navigation("Right");
                });

            modelBuilder.Entity("IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Issue", b =>
                {
                    b.Navigation("_relatedFrom");

                    b.Navigation("_relatedTo");
                });
#pragma warning restore 612, 618
        }
    }
}
