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

using IssueTracker.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Polly.Registry;

namespace IssueTracker.Infrastructure;

/// <summary>
/// Extension methods intended to add servcies to <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds <see cref="IPolicyRegistry{String}"/> to <paramref name="services"/>
    /// </summary>
    public static IServiceCollection AddPolicyRegistry(this IServiceCollection services)
    {
        return services.AddSingleton<IReadOnlyPolicyRegistry<string>>(BuildPolicyRegistry);
    }

    private static IReadOnlyPolicyRegistry<string> BuildPolicyRegistry(IServiceProvider provider)
    {
        PolicyRegistry registry = new();

        IEnumerable<IPolicyRegistryVisitor> visitors = provider.GetServices<IPolicyRegistryVisitor>();

        foreach (IPolicyRegistryVisitor visitor in visitors)
        {
            visitor.Visit(registry);
        }

        return registry;
    }
}
