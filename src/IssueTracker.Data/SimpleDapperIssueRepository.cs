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

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Core.Model;
using IssueTracker.Core.Projections;
using IssueTracker.Data.Abstractions;
using Microsoft.Data.Sqlite;
using Dapper;

namespace IssueTracker.Data;

public sealed class SimpleDapperIssueRepository
    : IIssueRepository
    , IDisposable
    , IAsyncDisposable
{
    private const int Disposed = 1;
    private readonly SqliteConnection _dbConnection;
    private int _disposed;


    public SimpleDapperIssueRepository(DatabaseOptions dbOptions)
    {
        if (dbOptions is null)
        {
            throw new ArgumentNullException(nameof(dbOptions));
        }

        _dbConnection = new SqliteConnection(dbOptions.ConnectionString);
    }

    /// <inheritdoc />
    public IAsyncEnumerable<IssueSummaryProjection> GetIssueSummaries(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        throw new NotImplementedException();
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
        ThrowIfDisposed();
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<Issue> GetIssueById(Guid id, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<Issue> AddIssue(Issue issue, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();

        await _dbConnection.ExecuteAsync(@"INSERT INTO Issues
(""Id"", ""Title"", ""Description"", ""Priority"", ""LastUpdated"", ""ConcurrencyToken"", ""Type"")
VALUES
(@Id, @Title, @Description, @Priority, @LastUpdated, @ConcurrencyToken, @Type)", issue);

        return issue;
    }

    /// <inheritdoc />
    public Task CommitAsync(CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<bool> DeleteIssueById(Guid id, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<bool> IssueExists(Guid id, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        throw new NotImplementedException();
    }

    ~SimpleDapperIssueRepository()
    {
        Dispose(false);
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

        if (disposing)
        {
        }

        _dbConnection.Dispose();
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
