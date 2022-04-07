using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using IssueTracker.App.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

[assembly: PreApplicationStartMethod(typeof(IssueTracker.App.WebApiApplication), "InitModule")]

namespace IssueTracker.App
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IServiceProvider _provider;

        public static void InitModule()
        {
            RegisterModule(typeof(ServiceScopeModule));
        }

        protected void Application_Start()
        {


            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            IServiceCollection services = new ServiceCollection();
            ServiceConfig.Configure(services);
            _provider = services.BuildServiceProvider();

            ServiceScopeModule.SetServiceProvider(_provider);

            DependencyResolver.SetResolver(new ServiceDependencyResolver());
        }

        protected void Application_End()
        {
            _provider = null;
        }
    }

}
