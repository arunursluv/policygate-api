using System.Collections.Concurrent;
using PolicyGate.Application.Abstractions;
using PolicyGate.Domain.Models;
namespace PolicyGate.Infrastructure.Repositories;

public sealed class InMemoryRuleConfigurationRepository : IRuleConfigurationRepository
{
    private readonly ConcurrentDictionary<string, SecurityRuleDefinition> definitions = new(StringComparer.OrdinalIgnoreCase);
    public InMemoryRuleConfigurationRepository(IEnumerable<SecurityRuleDefinition> initial)
    {
        foreach (var item in initial) definitions[item.RuleId] = item;
    }
    public bool IsEnabled(string ruleId) => definitions.TryGetValue(ruleId, out var value) && value.Enabled;
    public IReadOnlyCollection<SecurityRuleDefinition> GetAll() => definitions.Values.OrderBy(x => x.Order).ToArray();
    public bool Update(string ruleId, bool enabled)
    {
        if (!definitions.TryGetValue(ruleId, out var current))
            return false; definitions[ruleId] = current with { Enabled = enabled };
        return true;
    }
}
