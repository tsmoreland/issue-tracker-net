//
// Copyright © 2023 Terry Moreland
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

namespace IssueTracker.Shared.AspNetCore;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class OpenApiLinkAttribute : Attribute
{
    private readonly string[] _parameterNameValuePairCsv;
    public string OperationId { get; }
    public int ResponseCode { get; }

    public string? Description { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenApiLinkAttribute"/> class.
    /// </summary>
    /// <param name="operationId">The operation id of the linked operation</param>
    /// <param name="responseCode">The response code used to determine which response the link is associated with</param>
    /// <param name="parameterNameValuePairCsv">
    /// an array of comma separated name, expression values returned using <see cref="GetParameters"/>
    /// </param>
    public OpenApiLinkAttribute(string operationId, int responseCode, params string[] parameterNameValuePairCsv)
    {
        _parameterNameValuePairCsv = parameterNameValuePairCsv;
        OperationId = operationId;
        ResponseCode = responseCode;
    }

    public IEnumerable<(string Name, string Expression)> GetParameters()
    {
        return _parameterNameValuePairCsv
            .Select(csvPair => csvPair.Split(','))
            .Where(pair => pair is { Length: 2 })
            .Select(pair => (pair[0], pair[1]));
    }
}
