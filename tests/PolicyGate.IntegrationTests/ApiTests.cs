using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using PolicyGate.Application.Contracts;
using PolicyGate.Domain.Enums;
using Xunit;
namespace PolicyGate.IntegrationTests;

public sealed class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;
    public ApiTests(WebApplicationFactory<Program> factory) => this.factory = factory;
    [Fact]
    public async Task Evaluate_WithoutIdentity_ReturnsUnauthorized()
    {
        var response = await factory.CreateClient().PostAsJsonAsync("/api/security/evaluate", Request("hello"));
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    [Fact]
    public async Task Evaluate_NormalPrompt_ReturnsAllowed()
    {
        var client = Client("AIReader"); var response = await client.PostAsJsonAsync("/api/security/evaluate", Request("hello"));
        response.EnsureSuccessStatusCode(); (await response.Content.ReadFromJsonAsync<EvaluatePromptResponse>())!.Decision.Should().Be(Decision.Allowed);
    }
    [Fact]
    public async Task Evaluate_InvalidBody_ReturnsBadRequest()
    {
        var client = Client("AIReader");
        var response = await client.PostAsJsonAsync("/api/security/evaluate", new { model = "m", operation = "chat", prompt = "" });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    [Fact]
    public async Task Admin_CanDisableRule()
    {
        var client = Client("AIAdmin");
        var response = await client.PutAsJsonAsync("/api/rules/AI-PROMPT-001", new UpdateRuleRequest(false));
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    [Fact]
    public async Task NonAdmin_CannotDisableRule()
    {
        var client = Client("AIReader");
        var response = await client.PutAsJsonAsync("/api/rules/AI-PROMPT-001", new UpdateRuleRequest(false)); response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    private HttpClient Client(string roles)
    {
        var c = factory.CreateClient();
        c.DefaultRequestHeaders.Add("X-User-Id", "test-user");
        c.DefaultRequestHeaders.Add("X-User-Roles", roles);
        return c;
    }
    private static EvaluatePromptRequest Request(string prompt) => new() { Model = "internal-assistant", Operation = "chat", Prompt = prompt };
}
