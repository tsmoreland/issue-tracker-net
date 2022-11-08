//
// Copyright (c) 2022 Terry Moreland
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

using IssueTracker.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;

namespace IssueTracker.Issues.Infrastructure;

public sealed class IssueDataHealthCheck : IHealthCheck
{
    private readonly IDbContextFactory<IssuesDbContext> _dbContextFactory;
    private readonly ILogger<IssueDataHealthCheck> _logger;
    private readonly IAsyncPolicy<HealthCheckResult> _policy;

    public IssueDataHealthCheck(
        IDbContextFactory<IssuesDbContext> dbContextFactory,
        IReadOnlyPolicyRegistry<string> registry,
        ILoggerFactory loggerFactory)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

        ArgumentNullException.ThrowIfNull(registry, nameof(registry));
        _policy = registry.Get<IAsyncPolicy<HealthCheckResult>>(CommonPollyPolicyNames.HealthCheckResultCacheAsyncPolicy);

        ArgumentNullException.ThrowIfNull(registry, nameof(registry));
        _logger = loggerFactory.CreateLogger<IssueDataHealthCheck>();
    }

    public static string Name => "issues_db";
    public static IEnumerable<string> Tags => new[] { "ready" };

    private sealed record class HealthContextArguments(HealthCheckContext Context,
        IDbContextFactory<IssuesDbContext> DbContextFactory, ILogger Logger, CancellationToken CancellationToken);

    /// <inheritdoc />
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        const string operationKey = nameof(IssueDataHealthCheck) + "4F0D6308-818A-4791-9B6E-F65BCFB30485";

        HealthContextArguments arguments = new(context, _dbContextFactory, _logger, cancellationToken);
        return _policy.ExecuteAsync(static ctx =>
        {
            HealthContextArguments arguments = (HealthContextArguments)ctx["ctx"];

            (HealthCheckContext context, IDbContextFactory<IssuesDbContext> dbContextFactory, ILogger logger,
                CancellationToken cancellationToken) = arguments;
            return GetHealthCheckResult(context, dbContextFactory, logger, cancellationToken);

        }, new Context(operationKey, new Dictionary<string, object> { { "ctx", arguments } }));

    }

    private static Task<HealthCheckResult> GetHealthCheckResult(HealthCheckContext context, IDbContextFactory<IssuesDbContext> dbContextFactory, ILogger logger, CancellationToken cancellationToken)
    {
        Task<bool> task = CanConnect(dbContextFactory, logger, cancellationToken);
        return task.IsCompletedSuccessfully
            ? Task.FromResult(GetHealthCheckResult(context, task.Result))
            : GetHealthCheckResult(context, task);
    }

    private static Task<HealthCheckResult> GetHealthCheckResult(HealthCheckContext context, Task<bool> valueTask)
    {
        return valueTask
            .ContinueWith(static (t, state) =>
            {
                HealthCheckContext context = (HealthCheckContext)state!;
                bool canConnect = t.Result;
                return GetHealthCheckResult(null!, canConnect);
            }, context);
    }

    private static HealthCheckResult GetHealthCheckResult(HealthCheckContext context, bool canConnect)
    {
        return canConnect
            ? HealthCheckResult.Healthy("Issue database is connectable")
            : new HealthCheckResult(context.Registration.FailureStatus, "Unable to connection to Issue database");
    }

    /// <remarks>
    /// <para>
    /// async/await is the far more simple way and in most cases the preferred  way to do this, I'm choosing the more complex method
    /// in part to have a working example that shows that work, particularly one that has to deal with IDisposable's
    /// </para>
    /// <para>
    /// While this way is more complex is done offer one benefit - no closures which could keep objects alive longer than intended.
    /// </para>
    /// </remarks>
    private static Task<bool> CanConnect(IDbContextFactory<IssuesDbContext> dbContextFactory, ILogger logger, CancellationToken cancellationToken)
    {
        return dbContextFactory.CreateDbContextAsync(cancellationToken)
            .ContinueWith(static (t, state) =>
            {
                (CancellationToken cancellationToken, ILogger logger) = ((CancellationToken, ILogger))state!;
                IssuesDbContext context = t.Result;
                if (cancellationToken.IsCancellationRequested)
                {
                    context.Dispose();
                    if (t.IsFaulted)
                    {
                        logger.LogError(t.Exception, "Unexpected error occurred while testing connectivity");
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                }

                return context.Database.CanConnectAsync(cancellationToken)
                    .ContinueWith(static (t, state) =>
                    {
                        (IssuesDbContext context, ILogger logger) = ((IssuesDbContext, ILogger))state!;
                        context.Dispose();

                        if (t.IsFaulted)
                        {
                            logger.LogError(t.Exception, "Unexpected error occurred while testing connectivity");
                            throw t.Exception!;
                        }

                        if (t.IsCanceled)
                        {
                            throw new TaskCanceledException();
                        }

                        return t.Result;
                    }, (context, logger), cancellationToken);

            }, (cancellationToken, logger), TaskContinuationOptions.OnlyOnRanToCompletion)
            .Unwrap();
    }

}
