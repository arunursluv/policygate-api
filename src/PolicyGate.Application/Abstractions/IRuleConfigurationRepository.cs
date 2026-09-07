using PolicyGate.Domain.Models;
namespace PolicyGate.Application.Abstractions;
public interface IRuleConfigurationRepository
{
    bool IsEnabled(string ruleId);
    IReadOnlyCollection<SecurityRuleDefinition> GetAll();
    bool Update(string ruleId, bool enabled);
}
