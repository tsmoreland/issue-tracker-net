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

using System;
using System.Web;
using Microsoft.Extensions.DependencyInjection;

namespace IssueTracker.App.Soap
{
    /// <summary>
    /// Base class for webservices, used to setup <see cref="IServiceProvider"/>
    /// </summary>
    public abstract class WebServiceBase : System.Web.Services.WebService
    {
        /// <summary>
        /// <see cref="IServiceProvider"/>
        /// </summary>
        protected IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// initializes <see cref="ServiceProvider"/>
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// if unable to locate <see cref="IServiceProvider"/> in <see cref="HttpContext.Items"/>
        /// ></exception>
        protected WebServiceBase()
        {
            IServiceScope scope = (IServiceScope)HttpContext.Current?.Items[typeof(IServiceScope)];
            ServiceProvider = scope?.ServiceProvider ?? throw new InvalidOperationException("Missing Service Provider in Http Context items");
        }
    }
}
