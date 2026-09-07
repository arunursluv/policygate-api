using PolicyGate.Domain.Enums;
namespace PolicyGate.Domain.Models;
public sealed record AuditEvent(Guid EventId, DateTimeOffset TimestampUtc, string UserId, IReadOnlyCollection<string> Roles, string Model, string Operation, Decision Decision, int RiskScore, IReadOnlyCollection<string> MatchedRules, string PromptHash, string CorrelationId);
