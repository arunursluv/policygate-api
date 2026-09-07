using PolicyGate.Application.Abstractions;
using PolicyGate.Application.Models;
using PolicyGate.Domain.Enums;
using PolicyGate.Domain.Models;
namespace PolicyGate.Infrastructure.Rules;

public sealed class AuthenticatedIdentityRule : IAiSecurityRule
{
    public string RuleId => "AI-ID-001";
    public int Order => 10;
    public Task<SecurityRuleResult> EvaluateAsync(SecurityEvaluationContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var ok = !string.IsNullOrWhiteSpace(context.UserId) && context.UserId != "anonymous";
        return Task.FromResult(new SecurityRuleResult(RuleId, ok, ok ? RiskSeverity.None : RiskSeverity.Critical, ok ? "Authenticated identity found." : "Authenticated identity is required."));
    }
}
