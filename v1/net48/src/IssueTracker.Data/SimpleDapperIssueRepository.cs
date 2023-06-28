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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Core.Model;
using IssueTracker.Core.Projections;
using IssueTracker.Data.Abstractions;
using Microsoft.Data.Sqlite;
using Dapper;
using Dapper.Contrib.Extensions;

namespace IssueTracker.Data;

public sealed class SimpleDapperIssueRepository
    : IIssueRepository
    , IDisposable
    , IAsyncDisposable
{
    private const int Disposed = 1;
    private int _disposed;


    public SimpleDapperIssueRepository(DatabaseOptions dbOptions)
    {
        if (dbOptions is null)
        {
            throw new ArgumentNullException(nameof(dbOptions));
        }

        LazyDbConnection = new Lazy<SqliteConnection>(
            () =>
            {
                SqliteConnection dbConnection = new (dbOptions.ConnectionString);
                dbConnection.Open();
                return dbConnection;
            });
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<IssueSummaryProjection> GetIssueSummaries(int pageNumber, int pageSize, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ThrowIfDisposed();

        string query = $@"SELECT Id, Title, Priority, Type as IssueType
FROM
Issues
ORDER BY Title
LIMIT {pageSize} OFFSET {GetOffSet(pageNumber, pageSize)}";

        IEnumerable<IssueSummaryProjection> projections = await DbConnection.QueryAsync<IssueSummaryProjection>(query);
        foreach (IssueSummaryProjection projection in projections)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            yield return projection;
        }
    }

    /// <inheritdoc />
    public IAsyncEnumerable<IssueSummaryProjection> GetParentIssues(Guid id, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IAsyncEnumerable<IssueSummaryProjection> GetChildIssues(Guid id, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<Issue> GetUntrackedIssueById(Guid id, CancellationToken cancellationToken)
    {
        return GetIssueById(id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Issue> GetIssueById(Guid id, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();

        return await DbConnection.GetAsync<Issue>(id);
    }

    /// <inheritdoc />
    public async Task<Issue> AddIssue(Issue issue, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        ThrowIfArgumentNull(issue, nameof(issue));

        await DbConnection.InsertAsync(issue);
        return issue;
    }

    /// <inheritdoc />
    public async Task<Issue> UpdateIssue(Guid id, Action<Issue> visitor, CancellationToken cancellationToken)
    {
        Issue issue = await DbConnection.GetAsync<Issue>(id);
        if (issue is null)
        {
            return null;
        }

        visitor(issue);

        await DbConnection.UpdateAsync(issue);
        return issue;
    }

    /// <inheritdoc />
    public Task CommitAsync(CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteIssueById(Guid id, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        Issue issue = await DbConnection.GetAsync<Issue>(id);
        if (issue is null)
        {
            return false;
        }

        return await DbConnection.DeleteAsync(issue);
    }

    /// <inheritdoc />
    public async Task<bool> IssueExists(Guid id, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();

        return (await DbConnection.QueryAsync<int>("SELECT count(*) FROM Issue WHERE Id = @Id", id)).FirstOrDefault() > 0;
    }

    private Lazy<SqliteConnection> LazyDbConnection { get; }
    private SqliteConnection DbConnection => LazyDbConnection.Value;

    private static int GetOffSet(int pageNumber, int pageSize)
    {
        return (pageNumber - 1) * pageSize;
    }

    private static void ThrowIfArgumentNull(object value, string parameterName)
    {
        if (value is null)
        {
            throw new ArgumentNullException(parameterName);
        }
    }

    private void ThrowIfDisposed()
    {
        if (_disposed == Disposed)
        {
            throw new ObjectDisposedException("data connection has been closed.");
        }
    }

    ~SimpleDapperIssueRepository()
    {
        Dispose(false);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    private void Dispose(bool disposing)
    {
        int disposed = Interlocked.Exchange(ref _disposed, Disposed);
        if (disposed == Disposed)
        {
            return;
        }

        if (!disposing)
        {
            return;
        }

        if (LazyDbConnection.IsValueCreated)
        {
            LazyDbConnection.Value.Dispose();
        }

    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        int disposed = Interlocked.Exchange(ref _disposed, Disposed);
        if (disposed == Disposed)
        {
            return default;
        }

        // TODO:
        // for both this and Dispose we should wait on open method calls
        Dispose(true);
        return default;
    }
}
