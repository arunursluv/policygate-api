using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using PolicyGate.Application.Abstractions;
using PolicyGate.Application.Contracts;
using PolicyGate.Application.Models;
using PolicyGate.Domain.Enums;
using PolicyGate.Domain.Models;

namespace PolicyGate.Application.Services;

public sealed class SecurityEvaluationService(IEnumerable<IAiSecurityRule> rules, IRuleConfigurationRepository configurations, IAuditRepository auditRepository) : ISecurityEvaluationService
{
    public async Task<EvaluatePromptResponse> EvaluateAsync(EvaluatePromptRequest request, ClaimsPrincipal user, string correlationId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "annynomos";
        var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        var context = new SecurityEvaluationContext(userId, roles, request.Model, request.Operation, request.Prompt, request.ContextClassification);
        var results = new List<SecurityRuleResult>();
        var sanitizedPrompt = request.Prompt;

        foreach (var rule in rules.Where(r => configurations.IsEnabled(r.RuleId)).OrderBy(r => r.Order))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await rule.EvaluateAsync(context with { Prompt = sanitizedPrompt }, cancellationToken);
            results.Add(result);
            if (!string.IsNullOrWhiteSpace(result.SanitizedPrompt)) sanitizedPrompt = result.SanitizedPrompt;
        }

        var failures = results.Where(r => !r.Passed).ToArray();
        var riskScore = Math.Min(100, failures.Sum(r => r.Severity switch { RiskSeverity.Low => 10, RiskSeverity.Medium => 30, RiskSeverity.High => 60, RiskSeverity.Critical => 100, _ => 0 }));
        var decision = failures.Any(r => r.Severity >= RiskSeverity.High) ? Decision.Blocked : sanitizedPrompt != request.Prompt ? Decision.Masked : Decision.Allowed;
        var matchedRules = failures.Select(r => r.RuleId).ToArray();
        var message = decision switch
        { Decision.Allowed => "Request passed all enabled policies.", Decision.Masked => "Sensitive values were masked.", _ => "Request was blocked by security policy." };

        var audit = new AuditEvent(Guid.NewGuid(), DateTimeOffset.UtcNow, userId, roles, request.Model, request.Operation, decision, riskScore, matchedRules, Hash(request.Prompt), correlationId);
        await auditRepository.AddAsync(audit, cancellationToken);

        return new EvaluatePromptResponse(decision, riskScore, matchedRules, decision == Decision.Blocked ? null : sanitizedPrompt, message, correlationId);
    }

    private static string Hash(string value) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)));
}
