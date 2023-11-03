//
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
//

using IssueTracker.Issues.Domain.Configuration.ValueConverters;
using IssueTracker.Issues.Domain.DataContracts;
using IssueTracker.Issues.Infrastructure.Configurations;
using IssueTracker.Issues.Infrastructure.Configurations.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Issues.Infrastructure;

public sealed class IssuesDbContext : DbContext, IUnitOfWork
{
    private readonly IModelConfiguration _modelConfiguration;

    public IssuesDbContext(
        DbContextOptions<IssuesDbContext> options,
        IModelConfiguration modelConfiguration)
        : base(options)
    {
        _modelConfiguration = modelConfiguration ?? throw new ArgumentNullException(nameof(modelConfiguration));
    }

    public DbSet<Project> Projects => Set<Project>();

    public DbSet<Issue> Issues => Set<Issue>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Issue).Assembly);
        _modelConfiguration.ConfigureModel(modelBuilder);
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _modelConfiguration.ConfigureContext(optionsBuilder);
        base.OnConfiguring(optionsBuilder);
    }

    /// <inheritdoc />
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<IssueIdentifier>()
            .HaveConversion<IssueIdentifierValueConverter>();
        configurationBuilder
            .Properties<IssueIdentifier?>()
            .HaveConversion<NullableIssueIdentifierValueConverter>();
        configurationBuilder
            .Properties<DateTimeOffset?>()
            .HaveConversion<NullableDateTimeOffsetValueConverter>();
        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetValueConverter>();
    }

    /// <inheritdoc />
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        _modelConfiguration.SaveChangesVisitor(this);
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    /// <inheritdoc />
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        _modelConfiguration.SaveChangesVisitor(this);
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // raise events here, likely through MediatR, those event may contain entities tracked by this
        // context and those changes may need to be included here.

        await SaveChangesAsync(cancellationToken);
        return true;
    }
}
