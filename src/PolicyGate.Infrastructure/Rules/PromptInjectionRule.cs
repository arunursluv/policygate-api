using System.Text.RegularExpressions;
using PolicyGate.Application.Abstractions;
using PolicyGate.Application.Models;
using PolicyGate.Domain.Enums;
using PolicyGate.Domain.Models;
namespace PolicyGate.Infrastructure.Rules;

public sealed partial class PromptInjectionRule : IAiSecurityRule
{
    public string RuleId => "AI-PROMPT-001"; public int Order => 30;
    [GeneratedRegex(@"(?i)(ignore\s+(all\s+)?previous\s+instructions|reveal\s+(the\s+)?system\s+prompt|bypass\s+(security|restrictions)|act\s+as\s+(an\s+)?administrator)", RegexOptions.CultureInvariant)] 
    private static partial Regex InjectionPattern();
    public Task<SecurityRuleResult> EvaluateAsync(SecurityEvaluationContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var detected = InjectionPattern().IsMatch(context.Prompt);
        return Task.FromResult(new SecurityRuleResult(RuleId, !detected, detected ? RiskSeverity.Critical : RiskSeverity.None, detected ? "Potential prompt injection detected." : "No known prompt-injection pattern detected."));
    }
}
