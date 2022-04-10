using System;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using IssueTracker.App.Shared.Infrastructure;
using IssueTracker.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.Http.Description;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

[assembly: PreApplicationStartMethod(typeof(IssueTracker.App.WebApi.WebApiApplication), "InitModule")]

namespace IssueTracker.App.WebApi
{
    public class WebApiApplication : HttpApplication
    {
        private IServiceProvider _provider;

        public static void InitModule()
        {
            RegisterModule(typeof(ServiceScopeModule));
        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Filters.Add(new Filters.ExceptionToErrorResponseFilterAttribute());

            //switch from data contract serialization, needs to specify the serializer to use somehow
            //XmlMediaTypeFormatter xmlFormatter = GlobalConfiguration.Configuration.Formatters.XmlFormatter;
            //xmlFormatter.UseXmlSerializer = true;

            JsonMediaTypeFormatter jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            jsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy() { OverrideSpecifiedNames = false }, true));

            AreaRegistration.RegisterAllAreas();

            ApiDescriptionGroupCollection apiDescriptions = null;
            GlobalConfiguration.Configure(config => apiDescriptions = WebApiConfig.Register(config));
            GlobalConfiguration.Configure(config => SwaggerConfig.Register(config, apiDescriptions));

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            IServiceCollection services = new ServiceCollection();
            ServiceConfig.Configure(services);
            _provider = services.BuildServiceProvider();

            ServiceScopeModule.SetServiceProvider(_provider);
            DependencyResolver.SetResolver(new ServiceDependencyResolver());
            
            using (IServiceScope scope = _provider.CreateScope())
            {
                IIssueDataMigration migration =  scope.ServiceProvider.GetRequiredService<IIssueDataMigration>();
                try
                {
                    migration.Migrate();
                }
                catch (Exception ex)
                {
                    // TODO: replace with logging
                    Console.WriteLine(ex.ToString());
                    throw;
                }
            }

        }


        protected void Application_End()
        {
            _provider = null;
        }
    }

}
