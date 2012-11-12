using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CompanyGroup.WebClient
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "InvoiceInfoApi",
                routeTemplate: "api/{controller}/{action}/{paymentType}",
                defaults: new { action = "GetInvoiceInfo", paymentType = RouteParameter.Optional }
            );
        }
    }
}
