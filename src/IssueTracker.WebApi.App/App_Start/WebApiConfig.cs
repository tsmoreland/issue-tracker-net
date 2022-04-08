using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using IssueTracker.WebApi.App.Infrastructure;

namespace IssueTracker.WebApi.App
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            config.Services.Replace(typeof(IHttpControllerSelector),
                new CustomControllerSelector(config));
            config.Services.Replace(typeof(IHttpActionSelector),
                new CustomApiControllerActionSelector());


            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
