using PolicyGate.Application.Abstractions;
using PolicyGate.Application.Models;
using PolicyGate.Domain.Enums;
using PolicyGate.Domain.Models;
namespace PolicyGate.Infrastructure.Rules;

public sealed class ModelAccessRule : IAiSecurityRule
{
    public string RuleId => "AI-ID-002"; 
    public int Order => 20;
    public Task<SecurityRuleResult> EvaluateAsync(SecurityEvaluationContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var allowed = context.Roles.Any(r => r.Equals("AIReader", StringComparison.OrdinalIgnoreCase) || r.Equals("AIAnalyst", StringComparison.OrdinalIgnoreCase) || r.Equals("AIAdmin", StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(new SecurityRuleResult(RuleId, allowed, allowed ? RiskSeverity.None : RiskSeverity.High, allowed ? "Role permits model access." : "User role does not permit AI access."));
    }
}
