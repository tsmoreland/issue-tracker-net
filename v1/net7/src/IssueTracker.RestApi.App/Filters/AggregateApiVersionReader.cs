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

using Asp.Versioning;

namespace IssueTracker.RestApi.App.Filters;

/// <summary>
/// Aggregates multiple ApiVersionReaders allow others to serve as fallback
/// </summary>
public sealed class AggregateApiVersionReader : IApiVersionReader
{
    private readonly UrlSegmentApiVersionReader _urlSegmentApiVersionReader;
    private readonly HeaderApiVersionReader _headerApiVersionReader;
    private readonly QueryStringApiVersionReader _queryStringApiVersionReader;

    /// <summary>
    /// Instantiates a new instance of the <see cref="AggregateApiVersionReader"/> class.
    /// </summary>
    public AggregateApiVersionReader()
    {
        _urlSegmentApiVersionReader = new UrlSegmentApiVersionReader();
        _headerApiVersionReader = new HeaderApiVersionReader("X-Api-Version");
        _queryStringApiVersionReader = new QueryStringApiVersionReader("api-version");
    }

    /// <inheritdoc />
    public void AddParameters(IApiVersionParameterDescriptionContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _urlSegmentApiVersionReader.AddParameters(context);
        _headerApiVersionReader.AddParameters(context);
        _queryStringApiVersionReader.AddParameters(context);
    }

    /// <inheritdoc />
    public IReadOnlyList<string> Read(HttpRequest request)
    {
        IReadOnlyList<string> urlResult = _urlSegmentApiVersionReader.Read(request);
        IReadOnlyList<string> headerResult = _headerApiVersionReader.Read(request);
        IReadOnlyList<string> queryResult = _queryStringApiVersionReader.Read(request);

        return urlResult.Union(headerResult).Union(queryResult).AsEnumerable().Distinct().ToList().AsReadOnly();
    }
}
