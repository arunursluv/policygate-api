using System.Collections.Concurrent;
using PolicyGate.Application.Abstractions;
using PolicyGate.Domain.Models;
namespace PolicyGate.Infrastructure.Repositories;
public sealed class InMemoryAuditRepository : IAuditRepository
{
    private readonly ConcurrentQueue<AuditEvent> events = new();
    public Task AddAsync(AuditEvent auditEvent, CancellationToken cancellationToken = default) { cancellationToken.ThrowIfCancellationRequested(); events.Enqueue(auditEvent); return Task.CompletedTask; }
    public Task<IReadOnlyCollection<AuditEvent>> GetAllAsync(CancellationToken cancellationToken = default) { cancellationToken.ThrowIfCancellationRequested(); return Task.FromResult<IReadOnlyCollection<AuditEvent>>(events.OrderByDescending(x => x.TimestampUtc).ToArray()); }
    public Task ClearAsync(CancellationToken cancellationToken = default) { cancellationToken.ThrowIfCancellationRequested(); while(events.TryDequeue(out _)){} return Task.CompletedTask; }
}
