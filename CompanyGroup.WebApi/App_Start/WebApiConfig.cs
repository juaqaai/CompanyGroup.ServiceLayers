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

            //config.Routes.MapHttpRoute(
            //    name: "MaintainApi",
            //    routeTemplate: "api/{controller}/{action}/{dataAreaId}",
            //    defaults: new { dataAreaId = "hrp" }
            //);

            config.Routes.MapHttpRoute(
                name: "CustomerApi",
                routeTemplate: "api/{controller}/{action}/{dataAreaId}/{prefix}"
            );

            config.Routes.MapHttpRoute(
                name: "PictureItemApi",
                routeTemplate: "api/{controller}/{action}/{pictureId}/{maxWidth}/{maxHeight}",
                defaults: new { }
            );

            //config.Routes.MapHttpRoute(
            //    name: "InvoicePictureApi",
            //    routeTemplate: "api/{controller}/{action}/{recId}/{maxWidth}/{maxHeight}",
            //    defaults: new {}
            //);

            config.Routes.MapHttpRoute(
                name: "PictureApi",
                routeTemplate: "api/{controller}/{action}/{productId}/{recId}/{maxWidth}/{maxHeight}",
                defaults: new { dataAreaId = "hrp" }
            );
        }
    }
}
