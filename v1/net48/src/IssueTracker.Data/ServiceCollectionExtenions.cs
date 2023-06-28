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

//#define USE_IN_MEMORY_REPOSITORY

using System;
using IssueTracker.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace IssueTracker.Data
{
    public static class ServiceCollectionExtenions
    {
        public static IServiceCollection AddIssueData(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

#if USE_IN_MEMORY_REPOSITORY
            // normally a repository would be scoped but since this is in memory we'll use singleton
            services.AddSingleton<IIssueRepository, InMemoryIssueRepository>();
            services.AddTransient<IIssueDataMigration, InMemoryIssueDataMigration>();
#else
            services.AddScoped<IIssueRepository, SimpleDapperIssueRepository>();
            services.AddTransient<IIssueDataMigration, SimpleDapperIssueDataMigration>();
#endif

            return services;
        }
    }
}
