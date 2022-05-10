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

using IssueTracker.Issues.Domain.DataContracts;
using IssueTracker.Issues.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IssueTracker.Issues.Infrastructure;

public sealed class IssuesDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;
    private readonly ILogger<IssuesDbContext> _logger;

    public IssuesDbContext(
        DbContextOptions<IssuesDbContext> dbContextOptions,
        IConfiguration configuration,
        IHostEnvironment environment,
        ILoggerFactory loggerFactory)
        : base(dbContextOptions)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));

        ArgumentNullException.ThrowIfNull(loggerFactory, nameof(loggerFactory));
        _logger = loggerFactory.CreateLogger<IssuesDbContext>();

        ChangeTracker.StateChanged += ChangeTracker_StateChanged;
        ChangeTracker.Tracked += ChangeTracker_Tracked;
    }

    private static void ChangeTracker_Tracked(object? sender, Microsoft.EntityFrameworkCore.ChangeTracking.EntityTrackedEventArgs e)
    {
        if (e.Entry.Entity is IEntity entity)
        {
            RefreshIssueTimestamp(entity, e);
        }
    }

    private static void ChangeTracker_StateChanged(object? sender, Microsoft.EntityFrameworkCore.ChangeTracking.EntityStateChangedEventArgs e)
    {
        if (e.Entry.Entity is IEntity entity)
        {
            RefreshIssueTimestamp(entity, e);
        }
    }
    private static void RefreshIssueTimestamp(IEntity entity, EntityEntryEventArgs e)
    {
        if (e.Entry.State is EntityState.Added or EntityState.Modified)
        {
            entity.UpdateLastModifiedTime();
        }
    }


    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        string connectionString = _configuration.GetConnectionString("ApplicationConnection");
        optionsBuilder
            .UseSqlite(
                connectionString,
                options => options.MigrationsAssembly(typeof(IssuesDbContext).Assembly.FullName))
            .LogTo(message => _logger.LogInformation("{SQL}", message))
            .EnableSensitiveDataLogging(_environment.IsDevelopment());

        base.OnConfiguring(optionsBuilder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new IssueEntityTypeConfiguration());
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        ChangeTracker.StateChanged -= ChangeTracker_StateChanged;
        ChangeTracker.Tracked -= ChangeTracker_Tracked; 
        base.Dispose();
    }

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        ChangeTracker.StateChanged -= ChangeTracker_StateChanged;
        ChangeTracker.Tracked -= ChangeTracker_Tracked; 
        return base.DisposeAsync();
    }
}
