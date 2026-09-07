namespace PolicyGate.Application.Contracts;
public sealed record DashboardMetrics(int TotalRequests, int AllowedRequests, int MaskedRequests, int BlockedRequests, IReadOnlyDictionary<string,int> RuleHits);
