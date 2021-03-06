using System.Web.Http;
using WebActivatorEx;
using RESTfulXML_1;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace RESTfulXML_1
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "RESTfulXML");
                    })
                .EnableSwaggerUi(c => {});
        }
    }
}
