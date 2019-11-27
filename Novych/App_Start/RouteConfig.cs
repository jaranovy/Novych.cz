using System.Web.Mvc;
using System.Web.Routing;

namespace Novych
{
    public class RouteConfig
    {

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Jara",
                url: "Jara",
                defaults: new { controller = "Jara", action = "Index" }
            );


            routes.MapRoute(
                name: "Jaroslav",
                url: "Jaroslav",
                defaults: new { controller = "Jara", action = "Index" }
            );


            routes.MapRoute(
                name: "Jarousek",
                url: "Jarousek",
                defaults: new { controller = "Jara", action = "Index" }
            );

            routes.MapRoute(
                name: "Mirka",
                url: "Mirka",
                defaults: new { controller = "Mirka", action = "Index" }
                );


            routes.MapRoute(
                name: "Miroslava",
                url: "Miroslava",
                defaults: new { controller = "Mirka", action = "Index" }
            );


            routes.MapRoute(
                name: "Maja",
                url: "Maja",
                defaults: new { controller = "Mirka", action = "Index" }
            );


            routes.MapRoute(
                name: "Vyrobky",
                url: "Vyrobky/{action}",
                defaults: new { controller = "Vyrobky", action = "Index" }
            );

            routes.MapRoute(
                name: "Citerka",
                url: "Citerka",
                defaults: new { controller = "Citerka", action = "Index" }
                );

            routes.MapRoute(
                name: "ParniCistic",
                url: "ParniCistic",
                defaults: new { controller = "ParniCistic", action = "Index" }
                );

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );
        }
    }
}
