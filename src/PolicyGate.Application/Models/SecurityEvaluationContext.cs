namespace PolicyGate.Application.Models;
public sealed record SecurityEvaluationContext(string UserId, IReadOnlyCollection<string> Roles, string Model, string Operation, string Prompt, string ContextClassification);
