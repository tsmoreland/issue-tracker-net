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

using System.Collections.Immutable;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;

namespace IssueTracker.ServiceDiscovery;

internal record struct AssemblyLocation(Assembly Assembly, string Folder, string Fullpath)
{
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

    public IEnumerable<Assembly> GetAssembliesContainingType<T>(string rootNamespace)
    {
        ImmutableArray<AssemblyName> referencedAssemblyNames = Assembly.GetReferencedAssemblies()
            .Where(asm => asm.FullName.StartsWith(rootNamespace))
            .Union(new [] { Assembly.GetName() })
            .ToImmutableArray();

        List<AssemblyName> assemblyNames = GetRelatedAssemblyFilenames(rootNamespace)
            .Select(AssemblyName.GetAssemblyName)
            .Where(asm => referencedAssemblyNames.DoesNotContain(asm))
            .ToList();
        assemblyNames.ForEach(asm => Assembly.Load(asm));

        return assemblyNames
            .Select(Assembly.Load)
            .Where(ContainsType<T>);
    }

    public IEnumerable<AssemblyName> GetHostingStartupAssemblies(string rootNamespace) =>
        GetAssembliesContainingType<IHostingStartup>(rootNamespace)
            .Select(asm => asm.GetName());

    private IEnumerable<string> GetRelatedAssemblyFilenames(string rootNamespace)
    {
        return Directory.GetFiles(Folder, $"{rootNamespace}.*.dll");
    }

    private static bool ContainsType<T>(Assembly assembly)
    {
        return assembly.GetTypes().Any(type => typeof(T).IsAssignableFrom(type));
    }
}
