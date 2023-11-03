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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace IssueTracker.Shared.AspNetCore.ActionContraints;

[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
public sealed class RequestMatchesMediaTypeAttribute : Attribute, IActionConstraint
{
    private readonly string _requestHeaderToMatch;
    private readonly MediaTypeCollection _mediaTypes = new();

    /// <inheritdoc />
    public RequestMatchesMediaTypeAttribute(string requestHeaderToMatch, string mediaType, params string[] additionalMediaTypes)
    {
        _requestHeaderToMatch = requestHeaderToMatch ?? throw new ArgumentNullException(nameof(requestHeaderToMatch));

        _mediaTypes.Add(ParseOrThrow(mediaType));
        foreach (string additionalMediaType in additionalMediaTypes)
        {
            _mediaTypes.Add(ParseOrThrow(additionalMediaType));
        }

    }

    /// <inheritdoc />
    public bool Accept(ActionConstraintContext context)
    {
        IHeaderDictionary requestHeaders = context.RouteContext.HttpContext.Request.Headers;
        if (!requestHeaders.ContainsKey(_requestHeaderToMatch))
        {
            return false;
        }

        StringValues requestHeader = requestHeaders[_requestHeaderToMatch];
        var requestMediaType = new MediaType(requestHeader.ToString());

        return _mediaTypes
            .Select(mt => new MediaType(mt))
            .Contains(requestMediaType);
    }

    /// <inheritdoc />
    public int Order { get; } = 0;

    private static MediaTypeHeaderValue ParseOrThrow(string mediaType)
    {
        if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? parsedMediaType))
        {
            throw new ArgumentException("Invalid media type", nameof(mediaType));
        }

        return parsedMediaType;
    }
}
