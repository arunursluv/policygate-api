using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi;
using PolicyGate.Api.Authentication;
using PolicyGate.Application.Abstractions;
using PolicyGate.Application.Services;
using PolicyGate.Domain.Models;
using PolicyGate.Infrastructure.Repositories;
using PolicyGate.Infrastructure.Rules;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PolicyGate API",
        Version = "v1",
        Description = "Identity-aware AI security and compliance gateway."
    });
});
builder.Services.AddCors(o => o.AddPolicy("Angular", p => p.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddAuthentication("DemoHeaders").AddScheme<AuthenticationSchemeOptions, DemoHeaderAuthenticationHandler>("DemoHeaders", _ => { });
builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuditRepository, InMemoryAuditRepository>();
builder.Services.AddSingleton<IRuleConfigurationRepository>(_ => new InMemoryRuleConfigurationRepository(new[] {
    new SecurityRuleDefinition("AI-ID-001", "Authenticated Identity", "Requires a user identity.", true, 10),
    new SecurityRuleDefinition("AI-ID-002", "Model Access", "Requires AIReader, AIAnalyst, or AIAdmin role.", true, 20),
    new SecurityRuleDefinition("AI-PROMPT-001", "Prompt Injection", "Detects common instruction override attempts.", true, 30),
    new SecurityRuleDefinition("AI-DATA-001", "Sensitive Data", "Masks supported sensitive values.", true, 40)
}));
builder.Services.AddSingleton<IAiSecurityRule, AuthenticatedIdentityRule>();
builder.Services.AddSingleton<IAiSecurityRule, ModelAccessRule>();
builder.Services.AddSingleton<IAiSecurityRule, PromptInjectionRule>();
builder.Services.AddSingleton<IAiSecurityRule, SensitiveDataRule>();
builder.Services.AddScoped<ISecurityEvaluationService, SecurityEvaluationService>();

var app = builder.Build();
app.UseExceptionHandler("/error");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "PolicyGate API v1");

        options.RoutePrefix = "swagger";
        options.DocumentTitle = "PolicyGate API";
    });
}
app.UseCors("Angular");
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => Results.Redirect("/swagger"))
    .ExcludeFromDescription();
app.MapControllers();
app.Run();
public partial class Program { }
