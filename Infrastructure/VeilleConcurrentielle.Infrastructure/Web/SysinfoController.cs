using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace VeilleConcurrentielle.Infrastructure.Web
{
    public class SysinfoController : ApiControllerBase
    {
        private readonly IEnumerable<EndpointDataSource> _endpointSources;

        public SysinfoController(
            IEnumerable<EndpointDataSource> endpointSources
        )
        {
            _endpointSources = endpointSources;
        }

        /// <summary>
        /// Retrieved from https://stackoverflow.com/questions/28435734/how-to-get-a-list-of-all-routes-in-asp-net-core
        /// </summary>
        /// <returns></returns>
        [HttpGet("endpoints")]
        public async Task<ActionResult> GetAllEndpoints()
        {
            var endpoints = _endpointSources
                .SelectMany(es => es.Endpoints)
                .OfType<RouteEndpoint>();
            var output = endpoints.Select(
                e =>
                {
                    var controller = e.Metadata
                        .OfType<ControllerActionDescriptor>()
                        .FirstOrDefault();
                    var action = controller != null
                        ? $"{controller.ControllerName}.{controller.ActionName}"
                        : null;
                    var controllerMethod = controller != null
                        ? $"{controller.ControllerTypeInfo.FullName}:{controller.MethodInfo.Name}"
                        : null;
                    return new
                    {
                        Method = e.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault()?.HttpMethods?[0],
                        Route = $"/{e.RoutePattern.RawText.TrimStart('/')}",
                        Action = action,
                        ControllerMethod = controllerMethod
                    };
                }
            );

            return Ok(output);
        }
    }
}
