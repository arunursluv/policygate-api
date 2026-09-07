using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolicyGate.Application.Abstractions;
using PolicyGate.Domain.Models;
namespace PolicyGate.Api.Controllers;
[ApiController, Route("api/audit"), Authorize]
public sealed class AuditController(IAuditRepository repository) : ControllerBase
{
    [HttpGet] public async Task<ActionResult<IReadOnlyCollection<AuditEvent>>> Get(CancellationToken ct) => Ok(await repository.GetAllAsync(ct));
    [HttpDelete, Authorize(Roles="AIAdmin")] public async Task<IActionResult> Clear(CancellationToken ct) { await repository.ClearAsync(ct); return NoContent(); }
}
