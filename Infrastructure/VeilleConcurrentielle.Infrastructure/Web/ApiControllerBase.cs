using Microsoft.AspNetCore.Mvc;

namespace VeilleConcurrentielle.Infrastructure.Web
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}
