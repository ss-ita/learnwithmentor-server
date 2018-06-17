using System.Web.Http;
using System.Net.Http.Headers;
using System.Web.Http.Cors;
using System.Configuration;

namespace LearnWithMentor
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            string url = ConfigurationManager.AppSettings["ApiUrl"];
            config.EnableCors(new EnableCorsAttribute(url, headers: "*", methods: "*"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
