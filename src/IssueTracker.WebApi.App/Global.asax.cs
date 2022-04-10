using System;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using IssueTracker.WebApi.App.Infrastructure;
using IssueTracker.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.Http.Description;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

[assembly: PreApplicationStartMethod(typeof(IssueTracker.WebApi.App.WebApiApplication), "InitModule")]

namespace IssueTracker.WebApi.App
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

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            IServiceCollection services = new ServiceCollection();
            ServiceConfig.Configure(services);
            _provider = services.BuildServiceProvider();

            ServiceScopeModule.SetServiceProvider(_provider);
            DependencyResolver.SetResolver(new ServiceDependencyResolver());
            
            using (IServiceScope scope = _provider.CreateScope())
            {
                IIssueDataMigration migration =  scope.ServiceProvider.GetRequiredService<IIssueDataMigration>();
                migration.Migrate();
            }

        }


        protected void Application_End()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
            _provider = null;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name == "e_sqlite3")
            {
            }

            return Assembly.Load(args.Name);
        }

    }

}
