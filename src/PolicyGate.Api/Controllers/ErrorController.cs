using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
namespace PolicyGate.Api.Controllers;
[ApiController, ApiExplorerSettings(IgnoreApi=true)]
public sealed class ErrorController : ControllerBase
{
    [Route("/error"), AllowAnonymous] public IActionResult Error() 
    { 
        var feature = HttpContext.Features.Get<IExceptionHandlerFeature>(); 
        return Problem(title:"Unexpected server error", detail: feature?.Error.Message, statusCode:500); 
    }
}
