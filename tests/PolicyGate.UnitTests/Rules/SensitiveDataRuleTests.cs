using FluentAssertions;
using PolicyGate.Application.Models;
using PolicyGate.Infrastructure.Rules;
namespace PolicyGate.UnitTests.Rules;
public sealed class SensitiveDataRuleTests
{
    [Fact] public async Task Evaluate_Ssn_MasksValue() { var result = await new SensitiveDataRule().EvaluateAsync(Context("SSN 123-45-6789"), default); result.Passed.Should().BeFalse(); result.SanitizedPrompt.Should().Contain("***-**-****"); }
    [Fact] public async Task Evaluate_ApiKey_MasksValue() { var result = await new SensitiveDataRule().EvaluateAsync(Context("api_key=abc123"), default); result.SanitizedPrompt.Should().Contain("REDACTED"); }
    [Fact] public async Task Evaluate_NoSensitiveData_Passes() { var result = await new SensitiveDataRule().EvaluateAsync(Context("hello"), default); result.Passed.Should().BeTrue(); result.SanitizedPrompt.Should().BeNull(); }
    private static SecurityEvaluationContext Context(string p) => new("user1", ["AIReader"], "m", "chat", p, "Internal");
}
