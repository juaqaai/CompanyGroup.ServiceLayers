using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CompanyGroup.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //kontroller/metódus/paraméter (opcionális)
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "MaintainApi",
                routeTemplate: "api/{controller}/{action}/{dataAreaId}",
                defaults: new { dataAreaId = "hrp" }
            );

            config.Routes.MapHttpRoute(
                name: "CompletionApi",
                routeTemplate: "api/{controller}/{action}/{dataAreaId}/{prefix}/{completionType}",
                defaults: new { controller = "Product", action = "GetCompletionList", dataAreaId = "hrp" }
            );

            config.Routes.MapHttpRoute(
                name: "PictureApi",
                routeTemplate: "api/{controller}/{action}/{productId}/{recId}/{dataAreaId}/{maxWidth}/{maxHeight}",
                defaults: new { dataAreaId = "hrp" }
            );
        }
    }
}
