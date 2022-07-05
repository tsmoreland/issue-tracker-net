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

using System.Reflection;
using IssueTracker.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace IssueTracker.ServiceDiscovery;

public static class HostingStartupDiscovery
{
    private const string RootNamespace = "IssueTracker";

    public static IEnumerable<AssemblyName> DiscoverUnloadedAssembliesContainingHostingStartup()
    {
        AssemblyLocation? location = AssemblyLocation.FromAssembly(Assembly.GetEntryAssembly());
        return location is null
            ? Array.Empty<AssemblyName>()
            : location.Value.GetHostingStartupAssemblies(RootNamespace);
    }

    public static IEnumerable<AssemblyName> DiscoverUnloadedAssembliesContainingHostingStartup(in AssemblyLocation location)
    {
        return location.GetHostingStartupAssemblies(RootNamespace);
    }

    public static IEnumerable<Type> DiscoverTypes<T>(in AssemblyLocation location)
    {
        return location.DiscoverTypes<T>(RootNamespace);
    }

    public static IHealthChecksBuilder AddNamedHealthChecks(this IHealthChecksBuilder builder, in AssemblyLocation location)
    {
        IEnumerable<Type> healthCheckVisitors = DiscoverTypes<IHealthCheckBuilderVisitor>(in location);
        foreach (Type healthCheckVisitor in healthCheckVisitors)
        {
            // no support for structs, mostly trying to limit to something we can instantiate
            if (!healthCheckVisitor.IsClass)
            {
                continue;
            }

            IHealthCheckBuilderVisitor? visitor =
                (IHealthCheckBuilderVisitor?)Activator.CreateInstance(healthCheckVisitor, null);
            visitor?.Visit(builder);
        }
        return builder;
    }

}
