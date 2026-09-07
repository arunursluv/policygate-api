using FluentAssertions;
using PolicyGate.Domain.Enums;
using PolicyGate.Domain.Models;
using PolicyGate.Infrastructure.Repositories;
namespace PolicyGate.UnitTests.Repositories;
public sealed class InMemoryAuditRepositoryTests
{
    [Fact] public async Task AddAndGet_ReturnsEvent() { var repo = new InMemoryAuditRepository(); var e = Event(); await repo.AddAsync(e); (await repo.GetAllAsync()).Should().ContainSingle().Which.Should().Be(e); }
    [Fact] public async Task Clear_RemovesEvents() { var repo = new InMemoryAuditRepository(); await repo.AddAsync(Event()); await repo.ClearAsync(); (await repo.GetAllAsync()).Should().BeEmpty(); }
    [Fact] public async Task ConcurrentWrites_ArePreserved() { var repo = new InMemoryAuditRepository(); await Task.WhenAll(Enumerable.Range(0,100).Select(_=>repo.AddAsync(Event()))); (await repo.GetAllAsync()).Should().HaveCount(100); }
    private static AuditEvent Event()=>new(Guid.NewGuid(),DateTimeOffset.UtcNow,"u",["AIReader"],"m","chat",Decision.Allowed,0,[],"hash","corr");
}
