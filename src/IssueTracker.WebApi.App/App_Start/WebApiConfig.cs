using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using IssueTracker.WebApi.App.Infrastructure;
using Microsoft.Web.Http.Description;
using Microsoft.Web.Http.Routing;

namespace IssueTracker.WebApi.App
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
