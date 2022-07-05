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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace IssueTracker.Issues.Infrastructure;

public sealed class IssueDataHealthCheck : IHealthCheck
{
    private readonly IDbContextFactory<IssuesDbContext> _dbContextFactory;
    private readonly ILogger<IssueDataHealthCheck> _logger;

    public IssueDataHealthCheck(IDbContextFactory<IssuesDbContext> dbContextFactory, ILoggerFactory loggerFactory)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _logger = loggerFactory.CreateLogger<IssueDataHealthCheck>();
    }

    private async ValueTask<bool> CanConnect(CancellationToken cancellationToken)
    {
        try
        {
            await using IssuesDbContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await context.Database.CanConnectAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while testing connectivity");
            return false;
        }

    }

    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        bool canConnect = await CanConnect(cancellationToken);

        return canConnect
            ? HealthCheckResult.Healthy("Issue database is connectable")
            : new HealthCheckResult(context.Registration.FailureStatus, "Unable to connection to Issue database");
    }
}
