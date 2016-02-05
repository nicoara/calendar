using System.Web.Mvc;
using System.Web.Routing;

namespace Calendar.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Homepage", action = "Home", id = UrlParameter.Optional }
            );

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}