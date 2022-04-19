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
using IssueTracker.App.Shared.Extensions;
using IssueTracker.App.Shared.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

[assembly: PreApplicationStartMethod(typeof(IssueTracker.App.Soap.Global), "InitModule")]

namespace IssueTracker.App.Soap
{
    /// <summary>
    /// SOAP Web service application
    /// </summary>
    public class Global : HttpApplication
    {

        /// <summary>
        /// Initialize Module used to setup scoped services
        /// </summary>
        public static void InitModule()
        {
            RegisterModule(typeof(ServiceScopeModule));
        }

        /// <summary>
        /// Handle Application Start event.
        /// </summary>
        protected void Application_Start(object sender, EventArgs e)
        {
            IServiceCollection services = new ServiceCollection();
            ServiceConfig.Configure(services);
            IServiceProvider provider = services.BuildServiceProvider();
            ServiceScopeModule.SetServiceProvider(provider);

            provider.MigrateDatabaseOrThrow();
        }

        /// <summary>
        /// Handle Session Start event.
        /// </summary>
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handle Begin request event.
        /// </summary>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handle authenticate request event.
        /// </summary>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handle Application Error event.
        /// </summary>
        protected void Application_Error(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handle Session End event.
        /// </summary>
        protected void Session_End(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handle Application End event.
        /// </summary>
        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
