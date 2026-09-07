using PolicyGate.Domain.Models;
namespace PolicyGate.Application.Abstractions;
public interface IAuditRepository
{
    Task AddAsync(AuditEvent auditEvent, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AuditEvent>> GetAllAsync(CancellationToken cancellationToken = default);
    Task ClearAsync(CancellationToken cancellationToken = default);
}
