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

using IssueTracker.Shared;
using IssueTracker.Shared.Contracts;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly;
using Polly.Caching;
using Polly.Registry;

namespace IssueTracker.Infrastructure;

/// <inheritdoc cref="IPolicyRegistryVisitor"/>
public sealed class PolicyRegistryVisitor : IPolicyRegistryVisitor
{
    private readonly IAsyncCacheProvider _provider;

    /// <summary>
    /// Instantiates a new instance of <see cref="PolicyRegistryVisitor"/> class.
    /// </summary>
    public PolicyRegistryVisitor(IAsyncCacheProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    /// <inheritdoc />
    public void Visit(IPolicyRegistry<string> registry)
    {
        ArgumentNullException.ThrowIfNull(registry, nameof(registry));
        registry.Add(
            CommonPollyPolicyNames.HealthCheckResultCacheAsyncPolicy,
            Policy.CacheAsync(_provider.AsyncFor<HealthCheckResult>(), TimeSpan.FromSeconds(10)));

    }
}
