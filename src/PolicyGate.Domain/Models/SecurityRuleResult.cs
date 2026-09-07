using PolicyGate.Domain.Enums;
namespace PolicyGate.Domain.Models;
public sealed record SecurityRuleResult(string RuleId, bool Passed, RiskSeverity Severity, string Reason, string? SanitizedPrompt = null);
