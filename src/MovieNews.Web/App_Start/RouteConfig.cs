using System.Web.Mvc;
using System.Web.Routing;

namespace MovieNews.Web
{
	public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

	        routes.MapRoute(
		        name: "Index",
		        url: "",
		        defaults: new { controller = "Home", action = "Index" }
	        );

	        routes.MapRoute(
		        name: "Details",
		        url: "details/{title}",
		        defaults: new
		        {
			        controller = "Home",
			        action = "Details",
			        title = UrlParameter.Optional
		        }
	        );
		}
    }
}
