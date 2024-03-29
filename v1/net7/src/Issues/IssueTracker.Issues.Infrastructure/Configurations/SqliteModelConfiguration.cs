﻿//
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

using IssueTracker.Issues.Domain.DataContracts;
using IssueTracker.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IssueTracker.Issues.Infrastructure.Configurations;

public sealed class SqliteModelConfiguration : IModelConfiguration
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SqliteModelConfiguration> _logger;
    private readonly bool _isDevelopment;

    public SqliteModelConfiguration(
        IConfiguration configuration,
        IHostEnvironment environment,
        ILoggerFactory loggerFactory)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        ArgumentNullException.ThrowIfNull(loggerFactory);
        ArgumentNullException.ThrowIfNull(environment);

        _logger = loggerFactory.CreateLogger<SqliteModelConfiguration>();
        _isDevelopment = environment.IsDevelopment();
    }

    /// <inheritdoc />
    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqliteModelConfiguration).Assembly);
    }

    /// <inheritdoc />
    public void ConfigureContext(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        string connectionString = _configuration
            .GetConnectionString("ApplicationConnection") ?? throw new InvalidConfigurationException("missing ApplicationConnection entry.");
        optionsBuilder
            .UseSqlite(
                connectionString,
                options => options.MigrationsAssembly(typeof(SqliteModelConfiguration).Assembly.FullName))
            .LogTo(message => _logger.LogInformation("{SQL}", message))
            .EnableSensitiveDataLogging(_isDevelopment);


        if (!_isDevelopment)
        {
            optionsBuilder.UseModel(Data.CompiledModels.IssuesDbContextModel.Instance);
        }


    }

    /// <inheritdoc />
    public void SaveChangesVisitor(DbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        IEnumerable<Entity> entries = dbContext.ChangeTracker
            .Entries()
            .Where(e => e.State is EntityState.Modified or EntityState.Added)
            .Select(e => e.Entity)
            .OfType<Entity>();

        foreach (Entity entity in entries)
        {
            entity.UpdateLastModifiedTime();
        }
    }
}
