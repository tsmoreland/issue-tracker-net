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

using IssueTracker.App.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssueTracker.App.Data;

public sealed class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions, IConfiguration configuration)
        : base(dbContextOptions)
    {
        _configuration = configuration;
    }

    public DbSet<Issue> Issues { get; init; } = null!;

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        string connectionString = _configuration.GetConnectionString("ApplicationConnection");
        optionsBuilder
            .UseSqlite(connectionString, options => options.MigrationsAssembly(typeof(Program).Assembly.FullName));

        base.OnConfiguring(optionsBuilder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        EntityTypeBuilder<Issue> issueEntity = modelBuilder.Entity<Issue>();
        issueEntity.ToTable("Issues").HasKey(e => e.Id);
        issueEntity.HasIndex(e => e.Name);
        issueEntity.HasIndex(e => e.Priority);

        issueEntity.Property(e => e.Id).IsRequired();
        issueEntity.Property(e => e.Name).IsRequired().IsUnicode().HasMaxLength(200);
        issueEntity.Property(e => e.Description).IsUnicode().HasMaxLength(500);
        issueEntity.Property(e => e.Priority).IsRequired();
        issueEntity.Property("ConcurrencyToken").IsConcurrencyToken();

        issueEntity.HasData
        (
            new Issue(Guid.NewGuid(), "First", "First issue", Priority.Medium, new DateTime(2022, 01, 01), Guid.NewGuid().ToString()),
            new Issue(Guid.NewGuid(), "Second", "Second issue", Priority.Low, new DateTime(2022, 01, 02), Guid.NewGuid().ToString())
        );
    }
}
