using PolicyGate.Application.Models;
using PolicyGate.Domain.Models;
namespace PolicyGate.Application.Abstractions;
public interface IAiSecurityRule
{
    string RuleId { get; }
    int Order { get; }
    Task<SecurityRuleResult> EvaluateAsync(SecurityEvaluationContext context, CancellationToken cancellationToken);
}
