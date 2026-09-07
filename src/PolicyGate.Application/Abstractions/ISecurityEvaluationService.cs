using PolicyGate.Application.Contracts;
using System.Security.Claims;
namespace PolicyGate.Application.Abstractions;
public interface ISecurityEvaluationService
{
    Task<EvaluatePromptResponse> EvaluateAsync(EvaluatePromptRequest request, ClaimsPrincipal user, string correlationId, CancellationToken cancellationToken);
}
