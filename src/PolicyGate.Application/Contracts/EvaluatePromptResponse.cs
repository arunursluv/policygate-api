using PolicyGate.Domain.Enums;
namespace PolicyGate.Application.Contracts;
public sealed record EvaluatePromptResponse(Decision Decision, int RiskScore, IReadOnlyCollection<string> MatchedRules, string? SanitizedPrompt, string Message, string CorrelationId);
