using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.Application;
using System.Web.Http;

namespace RESTfulXML_1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
               name: "Swagger UI",
               routeTemplate: "",
               defaults: null,
               constraints: null,
               handler: new RedirectHandler(SwaggerDocsConfig.DefaultRootUrlResolver, "swagger/ui/index#/Requests"));

            var settings = config.Formatters.JsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Formatting = Formatting.Indented;

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "GetApi",
                routeTemplate: "api/jobs/saveFiles",
                defaults: new { controller = "Requests", action = "GetRequests" }
            );

            config.Routes.MapHttpRoute(
                name: "PostApi",
                routeTemplate: "api/data",
                defaults: new { controller = "Requests", action = "PostRequest" }
            );
        }
    }
}
