using Microsoft.AspNetCore.Mvc;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Controllers
{
    public interface IReceivedEventsController
    {
        Task<IActionResult> ReceiveEvent([FromBody] DispatchEventServerRequest request);
    }
}
