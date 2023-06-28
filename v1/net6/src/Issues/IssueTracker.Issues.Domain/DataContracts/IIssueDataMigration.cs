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

namespace IssueTracker.Issues.Domain.DataContracts;

/// <summary>
/// used to migrate the database on startup
/// </summary>
/// <remarks>
/// used for development purposes, not something we'd typically see in a production app
/// </remarks>
public interface IIssueDataMigration
{
    /// <summary>
    /// Migrate the database, updating it to the latest schema
    /// </summary>
    /// <param name="cancellationToken">A cancelation token</param>
    /// <returns>a <see cref="ValueTask"/></returns>
    ValueTask MigrateAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove existing database and reset back to initial state
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask ResetAndRepopultateAsync(CancellationToken cancellationToken = default);
}
