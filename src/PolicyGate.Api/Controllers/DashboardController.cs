using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolicyGate.Application.Abstractions;
using PolicyGate.Application.Contracts;
using PolicyGate.Domain.Enums;
namespace PolicyGate.Api.Controllers;

[ApiController, Route("api/dashboard"), Authorize]
public sealed class DashboardController(IAuditRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<DashboardMetrics>> Get(CancellationToken ct)
    {
        var events = await repository.GetAllAsync(ct);
        var hits = events.SelectMany(x => x.MatchedRules).GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
        return Ok(new DashboardMetrics(events.Count, events.Count(x => x.Decision == Decision.Allowed), events.Count(x => x.Decision == Decision.Masked), events.Count(x => x.Decision == Decision.Blocked), hits));
    }
}
