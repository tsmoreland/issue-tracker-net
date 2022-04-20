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
    [Migration("20220221214245_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.HasKey("Id");

                    b.HasIndex("Priority");

                    b.HasIndex("Title");

                    b.ToTable("Issues", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("1385056e-8afa-4e09-96df-ae12efdf1a29"),
                            ConcurrencyToken = "8ee1ef15-42b7-4dc2-843f-2bada4e449d7",
                            Description = "First issue",
                            LastUpdated = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Priority = 1,
                            Title = "First"
                        },
                        new
                        {
                            Id = new Guid("a28b8c45-6668-4169-940c-c16d71eb69de"),
                            ConcurrencyToken = "096122b5-3558-4430-b9e2-bff962596ae5",
                            Description = "Second issue",
                            LastUpdated = new DateTime(2022, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Priority = 0,
                            Title = "Second"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}