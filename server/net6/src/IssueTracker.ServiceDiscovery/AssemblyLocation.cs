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

using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;

namespace IssueTracker.ServiceDiscovery;

public readonly record struct AssemblyLocation(Assembly Assembly, string Folder, string Fullpath)
{
    // essentially violatile as it's not readonly 
    private readonly ConcurrentDictionary<string, HashSet<Assembly>> _assembliesByNamespace = new();

    public static AssemblyLocation? FromAssembly(Assembly? assembly)
    {
        if (assembly is null)
        {
            return null;
        }

        string? appFolder = Path.GetDirectoryName(assembly.Location);
        return appFolder is null
            ? null
            : new AssemblyLocation(assembly, appFolder, assembly.Location);
    }

    private readonly IEnumerable<Assembly> GetAssembliesForNamespace(string rootNameSspace)
    {
        Assembly assembly = Assembly;
        string folder = Folder;

        return _assembliesByNamespace.GetOrAdd(rootNameSspace,
            @namespace =>
            {
                ImmutableArray<AssemblyName> referencedAssemblyNames = assembly.GetReferencedAssemblies()
                    .Where(asm => asm.FullName.StartsWith(@namespace))
                    .Union(new[] { assembly.GetName() })
                    .ToImmutableArray();
                HashSet<Assembly> referencedAssemblylSet = GetRelatedAssemblyFilenames(folder, @namespace)
                    .Select(AssemblyName.GetAssemblyName)
                    .Where(asm => referencedAssemblyNames.DoesNotContain(asm))
                    .Select(Assembly.Load)
                    .Union(referencedAssemblyNames.Select(Assembly.Load))
                    .ToHashSet();
                return referencedAssemblylSet;
            });
    }

    public IEnumerable<Type> DiscoverTypes<T>(string rootNamespace)
    {
        return GetAssembliesForNamespace(rootNamespace)
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(T).IsAssignableFrom(t))
            .Distinct()
            .ToImmutableArray();
    }

    public readonly IEnumerable<Assembly> GetAssembliesContainingType<T>(string rootNamespace)
    {
        return GetAssembliesForNamespace(rootNamespace).Where(ContainsType<T>);
    }
    public static IEnumerable<Assembly> GetAssembliesContainingType<T>(Assembly assembly, string folder, string rootNamespace)
    {
        ImmutableArray<AssemblyName> referencedAssemblyNames = assembly.GetReferencedAssemblies()
            .Where(asm => asm.FullName.StartsWith(rootNamespace))
            .Union(new[] { assembly.GetName() })
            .ToImmutableArray();

        HashSet<Assembly> assemblies = GetRelatedAssemblyFilenames(folder, rootNamespace)
            .Select(AssemblyName.GetAssemblyName)
            .Where(asm => referencedAssemblyNames.DoesNotContain(asm))
            .Select(Assembly.Load)
            .Union(referencedAssemblyNames.Select(Assembly.Load))
            .ToHashSet();

        return assemblies
            .Where(ContainsType<T>);
    }

    public readonly IEnumerable<AssemblyName> GetHostingStartupAssemblies(string rootNamespace) =>
        GetAssembliesContainingType<IHostingStartup>(rootNamespace)
            .Select(asm => asm.GetName());

    private static IEnumerable<string> GetRelatedAssemblyFilenames(string folder, string rootNamespace)
    {
#if DEBUG
        string[] files = Directory.GetFiles(folder, $"{rootNamespace}.*.dll");
        return files;
#else
        return Directory.GetFiles(folder, $"{rootNamespace}.*.dll");
#endif
    }

    private static bool ContainsType<T>(Assembly assembly)
    {
        return assembly.GetTypes().Any(type => typeof(T).IsAssignableFrom(type));
    }
}
