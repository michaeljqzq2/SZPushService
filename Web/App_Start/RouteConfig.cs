﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Config",
                url: "c",
                defaults: new { controller = "Home", action = "Key" }
            );

            routes.MapRoute(
                name: "204",
                url: "204",
                defaults: new { controller = "Home", action = "Ping204" }
            );

            routes.MapRoute(
                name: "PingWebAppSCM",
                url: "p",
                defaults: new { controller = "Home", action = "PingWebAppSCM" }
            );

            routes.MapRoute(
                name: "SendEmail",
                url: "e",
                defaults: new { controller = "Home", action = "SendEmail" }
            );

            routes.MapRoute(
                name: "Single",
                url: "s",
                defaults: new { controller = "Home", action = "Single" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}