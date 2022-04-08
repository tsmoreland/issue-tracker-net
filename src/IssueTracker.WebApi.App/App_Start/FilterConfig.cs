using System.Web;
using System.Web.Mvc;

namespace IssueTracker.WebApi.App
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
