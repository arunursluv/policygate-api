using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolicyGate.Application.Abstractions;
using PolicyGate.Application.Contracts;
using PolicyGate.Domain.Models;
namespace PolicyGate.Api.Controllers;
[ApiController, Route("api/rules"), Authorize]
public sealed class RulesController(IRuleConfigurationRepository repository) : ControllerBase
{
    [HttpGet] 
    public ActionResult<IReadOnlyCollection<SecurityRuleDefinition>> Get() => Ok(repository.GetAll());
    [HttpPut("{ruleId}"), Authorize(Roles="AIAdmin")] 
    public IActionResult Update(string ruleId, UpdateRuleRequest request) => repository.Update(ruleId, request.Enabled) ? NoContent() : NotFound();
}
