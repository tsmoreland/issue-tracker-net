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

using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using IssueTracker.App.WebApi.Infrastructure;
using Microsoft.Web.Http.Description;
using Microsoft.Web.Http.Routing;

namespace IssueTracker.App.WebApi
{
    public static class WebApiConfig
    {
        public static ApiDescriptionGroupCollection Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Services.Replace(typeof(IHttpControllerSelector),
                new CustomControllerSelector(config));
            config.Services.Replace(typeof(IHttpActionSelector),
                new CustomApiControllerActionSelector());

            var constraintResolver = new DefaultInlineConstraintResolver
            {
                ConstraintMap = { ["apiVersion"] = typeof( ApiVersionRouteConstraint ) }
            };
            var httpServer = new HttpServer( config );

            // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
            config.AddApiVersioning( options => options.ReportApiVersions = true );
            VersionedApiExplorer apiExplorer = config.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'V";
                    options.SubstituteApiVersionInUrl = true;
                } );

            config.MapHttpAttributeRoutes(constraintResolver); 

            return apiExplorer.ApiDescriptions;
        }
    }
}
