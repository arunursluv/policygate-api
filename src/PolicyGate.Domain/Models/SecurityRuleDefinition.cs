namespace PolicyGate.Domain.Models;
public sealed record SecurityRuleDefinition(string RuleId, string Name, string Description, bool Enabled, int Order);
