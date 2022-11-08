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

using System;
using System.Web;
using Microsoft.Extensions.DependencyInjection;

namespace IssueTracker.App.Shared.Infrastructure
{
    public class ServiceScopeModule : IHttpModule
    {
        private static IServiceProvider s_serviceProvider;

        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            s_serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
            context.EndRequest += Context_EndRequest;
        }

        private static void Context_BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            context.Items[typeof(IServiceScope)] = s_serviceProvider.CreateScope();
        }
        private static void Context_EndRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            if (!context.Items.Contains(typeof(IServiceScope)))
            {
                return;
            }
            (context.Items[typeof(IServiceScope)] as IServiceScope)?.Dispose();
        }


        /// <inheritdoc />
        public void Dispose()
        {
            // no-op
        }

    }
}
