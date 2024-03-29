﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#pragma warning disable 219, 612, 618
#nullable enable

namespace IssueTracker.Issues.Infrastructure.Data.CompiledModels
{
    public partial class IssuesDbContextModel
    {
        partial void Initialize()
        {
            var issue = IssueEntityType.Create(this);
            var user = UserEntityType.Create(this);
            var user0 = User0EntityType.Create(this);
            var issueLink = IssueLinkEntityType.Create(this);
            var project = ProjectEntityType.Create(this);

            IssueEntityType.CreateForeignKey1(issue, issue);
            IssueEntityType.CreateForeignKey2(issue, project);
            UserEntityType.CreateForeignKey1(user, issue);
            User0EntityType.CreateForeignKey1(user0, issue);
            IssueLinkEntityType.CreateForeignKey1(issueLink, issue);
            IssueLinkEntityType.CreateForeignKey2(issueLink, issue);

            IssueEntityType.CreateAnnotations(issue);
            UserEntityType.CreateAnnotations(user);
            User0EntityType.CreateAnnotations(user0);
            IssueLinkEntityType.CreateAnnotations(issueLink);
            ProjectEntityType.CreateAnnotations(project);

            AddAnnotation("ProductVersion", "7.0.0");
        }
    }
}
