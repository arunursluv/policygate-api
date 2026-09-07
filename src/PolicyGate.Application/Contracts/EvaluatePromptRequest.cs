using System.ComponentModel.DataAnnotations;
namespace PolicyGate.Application.Contracts;
public sealed class EvaluatePromptRequest
{
    [Required, StringLength(100)] public string Model { get; init; } = "internal-assistant";
    [Required, StringLength(50)] public string Operation { get; init; } = "chat";
    [Required, StringLength(10000, MinimumLength = 1)] public string Prompt { get; init; } = string.Empty;
    [StringLength(50)] public string ContextClassification { get; init; } = "Internal";
}
