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
using System.Text.Json.Serialization;
using IssueTracker.Shared;

namespace IssueTracker.ServiceDiscovery;

public static class JsonSerializerContextDiscovery
{
    private const string RootNamespace = "IssueTracker";

    public static IEnumerable<IJsonSerializerOptionsConfiguration> DiscoverSerializerContextConfiguration()
    {
        AssemblyLocation? location = AssemblyLocation.FromAssembly(Assembly.GetEntryAssembly());
        return location is null
            ? Array.Empty<IJsonSerializerOptionsConfiguration>()
            : location.Value
                .DiscoverTypes<IJsonSerializerOptionsConfiguration>(RootNamespace)
                .Where(t => t.FullName is {Length: > 0})
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Select(t => t.Assembly.CreateInstance(t.FullName!))
                .OfType<IJsonSerializerOptionsConfiguration>();
    }

    public static IEnumerable<IJsonSerializerOptionsConfiguration> DiscoverSerializerContextConfiguration(in AssemblyLocation location)
    {
        return location
            .DiscoverTypes<IJsonSerializerOptionsConfiguration>(RootNamespace)
            .Where(t => t.FullName is {Length: > 0})
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .Select(t => t.Assembly.CreateInstance(t.FullName!))
            .OfType<IJsonSerializerOptionsConfiguration>();
    }
}
