using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolicyGate.Application.Abstractions;
using PolicyGate.Application.Contracts;
namespace PolicyGate.Api.Controllers;
[ApiController, Route("api/security"), Authorize]
public sealed class SecurityController(ISecurityEvaluationService service) : ControllerBase
{
    [HttpPost("evaluate")]
    public async Task<ActionResult<EvaluatePromptResponse>> Evaluate(EvaluatePromptRequest request, CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.TraceIdentifier;
        var response = await service.EvaluateAsync(request, User, correlationId, cancellationToken);
        return Ok(response);
    }
}
