//using System.Text.RegularExpressions;
//using PolicyGate.Application.Abstractions;
//using PolicyGate.Application.Models;
//using PolicyGate.Domain.Enums;
//using PolicyGate.Domain.Models;
//namespace PolicyGate.Infrastructure.Rules;

//public sealed partial class SensitiveDataRule : IAiSecurityRule
//{
//    public string RuleId => "AI-DATA-001"; 
//    public int Order => 40;
//    [GeneratedRegex(@"\b\d{3}-\d{2}-\d{4}\b", RegexOptions.CultureInvariant)] 
//    private static partial Regex Ssn();

//    [GeneratedRegex(@"(?i)(api[_-]?key|password|secret)\s*[:=]\s*[^\s,;]+", RegexOptions.CultureInvariant)]
//    private static partial Regex Secret();
//    public Task<SecurityRuleResult> EvaluateAsync(SecurityEvaluationContext context, CancellationToken cancellationToken)

//    {
//        cancellationToken.ThrowIfCancellationRequested(); var masked = Ssn().Replace(context.Prompt, "***-**-****");
//        masked = Secret().Replace(masked, "$1=***REDACTED***");
//        var changed = masked != context.Prompt;
//        return Task.FromResult(new SecurityRuleResult(RuleId, !changed, changed ? RiskSeverity.Medium : RiskSeverity.None, changed ? "Sensitive values were masked." : "No supported sensitive-data pattern found.", changed ? masked : null));
//    }
//}

using System.Text.RegularExpressions;
using PolicyGate.Application.Abstractions;
using PolicyGate.Application.Models;
using PolicyGate.Domain.Enums;
using PolicyGate.Domain.Models;

namespace PolicyGate.Infrastructure.Rules;

public sealed partial class SensitiveDataRule
    : IAiSecurityRule
{
    public string RuleId =>
        "AI-DATA-001";

    public int Order =>
        40;


    [GeneratedRegex(
        @"\b\d{3}-\d{2}-\d{4}\b",
        RegexOptions.CultureInvariant)]
    private static partial Regex Ssn();


    [GeneratedRegex(
        @"\b(api[_-]?key|password|secret)\s*[:=]\s*[^\s,;]+",
        RegexOptions.IgnoreCase |
        RegexOptions.CultureInvariant)]
    private static partial Regex Secret();


    public Task<SecurityRuleResult> EvaluateAsync(
        SecurityEvaluationContext context,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var originalPrompt =
            context.Prompt ?? string.Empty;


        var maskedPrompt =
            Ssn().Replace(
                originalPrompt,
                "***-**-****"
            );


        maskedPrompt =
            Secret().Replace(
                maskedPrompt,
                "$1=***REDACTED***"
            );


        var sensitiveDataDetected =
            !string.Equals(
                originalPrompt,
                maskedPrompt,
                StringComparison.Ordinal
            );


        if (!sensitiveDataDetected)
        {
            return Task.FromResult(
                new SecurityRuleResult(
                    RuleId,
                    true,
                    RiskSeverity.None,
                    "No supported sensitive-data pattern found.",
                    null
                )
            );
        }


        return Task.FromResult(
            new SecurityRuleResult(
                RuleId,
                false,
                RiskSeverity.Medium,
                "Sensitive values were detected and masked.",
                maskedPrompt
            )
        );
    }
}
